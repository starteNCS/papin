using System.Diagnostics;
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
            Stopwatch sw = Stopwatch.StartNew();
            var httpRequest = await AnalyzeHttpFromSocket(clientSocket, tokenSource.Token);
            var possibleRoutes = _routes.Where(r => r.Uri == httpRequest.Uri).ToList();
            if (!possibleRoutes.Any())
            {
                var responseNotFound = new HttpResponseBuilder()
                    .SetStatus(HttpStatus.NotFound)
                    .Build();
                clientSocket.SendHttpResponse(responseNotFound);
                return;
            }

            var route = possibleRoutes.FirstOrDefault(pr => pr.Method == httpRequest.Method);
            if (route == null)
            {
                var responseNotFound = new HttpResponseBuilder()
                    .SetStatus(HttpStatus.MethodNotAllowed)
                    .Build();
                clientSocket.SendHttpResponse(responseNotFound);
                return;
            }

            HttpResponse response = await route.Handler(httpRequest);

            clientSocket.SendHttpResponse(response);
            clientSocket.Close();
            sw.Stop();
            Logger.WriteInfo($"Successfully handled request in {sw.ElapsedMilliseconds}ms");
        }
        catch (OperationCanceledException)
        {
            var response = new HttpResponseBuilder()
                .SetStatus(HttpStatus.RequestTimeOut)
                .Build();
            clientSocket.SendHttpResponse(response);
            clientSocket.Close();
            Logger.WriteError("Request timed out");
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
                // + 4 for the line separator between
                builder.SetBodyStartIndex(index + 4);
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
