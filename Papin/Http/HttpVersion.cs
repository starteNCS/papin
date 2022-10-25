// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global
namespace Papin.Http;

public enum HttpVersion
{
    HTTP0_9,
    HTTP1_0,
    HTTP1_1,
    HTTP2_0,
    HTTP3_0
}

public static class HttpVersionExtensions
{
    public static HttpVersion ToHttpVersion(this string version)
    {
        return version switch
        {
            "HTTP/0.9" => HttpVersion.HTTP0_9,
            "HTTP/1.0" => HttpVersion.HTTP1_0,
            "HTTP/1.1" => HttpVersion.HTTP1_1,
            "HTTP/2.0" => HttpVersion.HTTP2_0,
            "HTTP/3.0" => HttpVersion.HTTP3_0,
            _ => throw new ArgumentException("Unsupported HTTP Version received")
        };
    }
}