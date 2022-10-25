using System.Net;
using System.Net.Sockets;
using Papin.Http;
using Papin.Utils;
using Papin.Utils.Algorithms;

namespace Papin.WebHost;

public class WebHost : IWebHost
{
    private readonly Dictionary<string, Action> _routes;
    
    private readonly Socket _serverSocket;

    public WebHost(Dictionary<string, Action> routes)
    {
        _routes = routes;
        
        _serverSocket = new Socket(IPAddress.Any.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        _serverSocket.Bind(new IPEndPoint(IPAddress.Loopback, 5000));
        _serverSocket.Listen(10);
    }
    
    public async Task Start()
    {
        Logger.WriteInfo("Starting Webhost... Waiting for connections");
        while (true)
        {
            var clientSocket = await _serverSocket.AcceptAsync();
            Logger.WriteInfo($"Accepted connection request from {clientSocket.RemoteEndPoint!}");
            var httpRequest = await AnalyzeHttpFromSocket(clientSocket);
        }
    }

    private static async Task<HttpRequest> AnalyzeHttpFromSocket(Socket clientSocket)
    {
        HttpRequestBuilder builder = new();
        var buffer = new byte[1024];
        int? httpPackageSize = null;
        while (true)
        {
            var receivedBytes = await clientSocket.ReceiveAsync(buffer);
            builder.Bytes.AddRange(buffer[..receivedBytes]);
            if (builder.Bytes.FindSequence(HttpRequest.HeaderEntityBodySeparator, out int index))
            {
                builder.ParseRequestMessageHeader(builder.Bytes.GetRange(0, index).ToArray());
                // content length only indicates the size of the entity body + 4 for the line separator between
                // MessageHeader and EntityBody
                httpPackageSize = builder.GetContentLength() + builder.HeaderLength + 4;
            }

            if (receivedBytes != 1024)
            {
                break;
            }
            
            if (httpPackageSize != null && httpPackageSize == builder.Bytes.Count)
            {
                break;
            }            
        }

        return builder.Build();
    }
}