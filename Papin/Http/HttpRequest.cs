using System.Collections.Immutable;

namespace Papin.Http;

public record HttpRequest
{
    public HttpMethod Method { get; init; }
    public string Uri { get; init; } = string.Empty;
    public HttpVersion Version { get; init; }

    public ImmutableList<HttpHeader>? Headers { get; set; }
}