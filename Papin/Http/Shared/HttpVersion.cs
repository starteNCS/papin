// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global
namespace Papin.Http.Shared;

public sealed record HttpVersion
{
    public const string HTTP0_9 = "HTTP/0.9";
    public const string HTTP1_0 = "HTTP/1.0";
    public const string HTTP1_1 = "HTTP/1.1";
    public const string HTTP2_0 = "HTTP/2.0";
    public const string HTTP3_0 = "HTTP/3.0";
}