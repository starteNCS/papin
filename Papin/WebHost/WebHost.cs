using System.Net;
using System.Net.Sockets;
using Papin.Response;
using Papin.Utils;

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
    
    public void Start()
    {
        while (true)
        {
            var clientSocket = _serverSocket.Accept();
            Logger.WriteInfo($"Connection accepted from {clientSocket.RemoteEndPoint!}");
        }
    }
}