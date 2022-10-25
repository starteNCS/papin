using System.Collections.Immutable;

namespace Papin.Http;

public class HttpRequestBuilder
{
    private HttpMethod? _method;
    private string? _uri;
    private HttpVersion? _version;

    private IEnumerable<HttpHeader>? _headers;
    
    public HttpRequestBuilder(string request)
    {
    }

    /// <summary>
    /// Parses the Request-Line
    /// https://www.rfc-editor.org/rfc/rfc1945#section-5.1
    /// </summary>
    /// <param name="requestLine">A valid request-line consisting of a Method, the URI and the Http version</param>
    /// <exception cref="ArgumentException">An invalid request-line was provided</exception>
    /// TODO: Return http 501 when a method is not implemented
    public void ParseRequestLine(string requestLine)
    {
        var parts = requestLine.Split(' ');
        if (parts.Length != 3)
        {
            throw new ArgumentException("Invalid Request-Line received");
        }

        _method = parts[0].ToHttpMethod();
        _uri = parts[1];
        _version = parts[2].ToHttpVersion();
    }

    /// <summary>
    /// Parses the Header fields
    /// https://www.rfc-editor.org/rfc/rfc1945#section-5.2
    /// </summary>
    /// <param name="headerFields">A valid list of header fields</param>
    /// <exception cref="ArgumentException">An invalid request-line was provided</exception>
    public void ParseHeaderFields(string[] headerFields)
    {
        _headers = headerFields.Select(header =>
        {
            var parts = header.Replace(": ", ":").Split(":");
            if (parts.Length != 2)
            {
                throw new ArgumentException("Invalid Header received");
            }
            
            return new HttpHeader
            {
                Key = parts[0],
                Value = parts[1]
            };
        });
    }

    /// <summary>
    /// Checks if all fields are checked and if those are filled with correct values
    /// </summary>
    /// <returns>Whether the HttpRequest is valid</returns>
    private bool IsBuildValid()
    {
        // check Request-Line
        if (_method is null || _uri is null || _version is null)
        {
            return false;
        }

        if (_version != HttpVersion.HTTP1_0)
        {
            return false;
        }

        return true;
    }
    
    /// <summary>
    /// Builds a HttpRequest with all the collected data
    /// </summary>
    /// <returns>A valid HttpRequest Object</returns>
    /// <exception cref="ArgumentException">Thrown when the http request is invalid</exception>
    public HttpRequest Build()
    {
        if (!IsBuildValid())
        {
            throw new ArgumentException("Invalid HttpRequest build");
        }

        return new HttpRequest
        {
            Method = _method!.Value,
            Uri = _uri!,
            Version = _version!.Value,
            Headers = _headers?.ToImmutableList()
        };
    }
}