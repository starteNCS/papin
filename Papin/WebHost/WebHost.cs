using System.Net;
using System.Net.Sockets;
using System.Text;
using Papin.Utils;

namespace Papin.WebHost;

public class WebHost : IWebHost
{
    private readonly Dictionary<string, Action> _routes;
    
    private readonly Socket _serverSocket;
    private readonly WaitHandle _waitHandle;

    public WebHost(Dictionary<string, Action> routes)
    {
        _routes = routes;
        
        _serverSocket = new Socket(IPAddress.Any.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        _serverSocket.Bind(new IPEndPoint(IPAddress.Loopback, 5000));
        _serverSocket.Listen(10);
        _waitHandle = new Mutex();
    }
    
    public async Task Start()
    {
        Logger.WriteInfo("Starting Webhost... Waiting for connections");
        while (true)
        {
            var clientSocket = await _serverSocket.AcceptAsync();
            Logger.WriteInfo($"Accepted connection request from {clientSocket.RemoteEndPoint!}");
            await Task.Run(async () => await AnalyzeHttpFromSocket(clientSocket));
        }
    }

    private static async Task AnalyzeHttpFromSocket(Socket clientSocket)
    {
        byte[] buffer = new byte[1024];
        var c = await clientSocket.ReceiveAsync(buffer);
        Console.WriteLine(Encoding.UTF8.GetString(buffer));
    }
}