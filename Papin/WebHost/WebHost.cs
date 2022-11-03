using System.Net;
using System.Net.Sockets;
using System.Text;
using Papin.Http;
using Papin.Http.Request;
using Papin.Http.Response;
using Papin.Utils;
using Papin.Utils.Algorithms;
using Papin.Utils.Models;

namespace Papin.WebHost;

public class WebHost : IWebHost
{
    private readonly List<Route> _routes;
    
    private readonly Socket _serverSocket;

    public WebHost(List<Route> routes)
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
            var tokenSource = new CancellationTokenSource(10_000_000);
            var clientSocket = await _serverSocket.AcceptAsync(CancellationToken.None);
            Logger.WriteInfo($"Accepted connection request from {clientSocket.RemoteEndPoint!}");
            await HandleScope(clientSocket, tokenSource);
        }
    }

    private async Task HandleScope(Socket clientSocket, CancellationTokenSource tokenSource)
    {
        try
        {
            var httpRequest = await AnalyzeHttpFromSocket(clientSocket, tokenSource.Token);
            var route = _routes.SingleOrDefault(r => r.Uri == httpRequest.Uri);
            if (route == null)
            {
                var responseNotFound = new HttpResponseBuilder()
                    .SetStatus(HttpStatus.NotFound)
                    .Build();
                clientSocket.SendHttpResponse(responseNotFound);
                return;
            }

            if (route.Method != httpRequest.Method)
            {
                var responseNotFound = new HttpResponseBuilder()
                    .SetStatus(HttpStatus.MethodNotAllowed)
                    .Build();
                clientSocket.SendHttpResponse(responseNotFound);
                return;
            }

            HttpResponse response = route.Handler();

            clientSocket.SendHttpResponse(response);
            clientSocket.Close();
        }
        catch (OperationCanceledException)
        {
            // todo: send timeout
            var response = new HttpResponseBuilder()
                .SetStatus(HttpStatus.RequestTimeOut)
                .Build();
            clientSocket.SendHttpResponse(response);
            clientSocket.Close();
        }
    }

    private static async Task<HttpRequest> AnalyzeHttpFromSocket(Socket clientSocket, CancellationToken token)
    {
        HttpRequestBuilder builder = new();
        var buffer = new byte[1024];
        int? httpPackageSize = null;
        while (true)
        {
            var receivedBytes = await clientSocket.ReceiveAsync(buffer, token);
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