using System.Net.Sockets;
using System.Text;
using Papin.Http.Response;

namespace Papin.Utils;

public static class SocketUtils
{
    public static void SendHttpResponse(this Socket socket, HttpResponse response)
    {
        socket.Send(Encoding.ASCII.GetBytes(response.ToString()));
        socket.Close();
    }
}