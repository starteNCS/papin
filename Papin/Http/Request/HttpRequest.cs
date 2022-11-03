using System.Collections.Immutable;

namespace Papin.Http.Request;

public record HttpRequest
{
    /// <summary>
    /// A sequence of bytes, that separate the headers from the body
    /// Represents one empty line
    /// CRLF CRLF
    /// </summary>
    public static byte[] HeaderEntityBodySeparator = { 13, 10, 13, 10 };
    
    public HttpMethod Method { get; init; }
    public string Uri { get; init; } = string.Empty;
    public HttpVersion Version { get; init; }

    public ImmutableList<HttpHeader>? Headers { get; init; }
    
    public ImmutableList<byte>? RawBytes { get; init; }
}