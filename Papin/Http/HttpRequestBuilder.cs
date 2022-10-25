using System.Collections.Immutable;
using System.Text;

namespace Papin.Http;

public class HttpRequestBuilder
{
    private HttpMethod? _method;
    private string? _uri;
    private HttpVersion? _version;

    private IEnumerable<HttpHeader>? _headers;

    public List<byte> Bytes { get; } = new();
    public int? HeaderLength { get; private set; }

    /// <summary>
    /// Start the http request builder using a full plain text http request
    /// </summary>
    /// <param name="request">Fully received http request</param>
    public HttpRequestBuilder(string request)
    {
    }

    /// <summary>
    /// Start the http request and build it by hand
    /// </summary>
    public HttpRequestBuilder()
    {
    }

    /// <summary>
    /// Parses the Request Message Header (Request-Line and Headers)
    /// </summary>
    public void ParseRequestMessageHeader(byte[] bytes)
    {
        HeaderLength = bytes.Length;
        ParseRequestMessageHeader(Encoding.ASCII.GetString(bytes));
    }
    
    /// <summary>
    /// Parses the Request Message Header (Request-Line and Headers)
    /// </summary>
    public void ParseRequestMessageHeader(string requestMessageHeader)
    {
        if (HeaderLength == null)
        {
            HeaderLength = Encoding.ASCII.GetByteCount(requestMessageHeader);
        }
        
        var lines = requestMessageHeader.Split("\r\n");
        
        ParseRequestLine(lines[0]);
        ParseHeaderFields(lines[1..]);
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
            var separator = header.IndexOf(':');
            if (separator == -1)
            {
                throw new ArgumentException("Invalid Header received");
            }
            
            return new HttpHeader
            {
                Key = header[..separator],
                // + 2 for the colon itself and the space character
                Value = header[(separator + 2)..]
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

        if (_version != HttpVersion.HTTP1_1)
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
            Headers = _headers?.ToImmutableList(),
            RawBytes = Bytes.ToImmutableList()
        };
    }

    /// <summary>
    /// Quality of Life Method, that returns the content length header before building the HttpRequest
    /// </summary>
    /// <returns>The content length</returns>
    public int GetContentLength()
    {
        if (_headers == null)
        {
            throw new ArgumentNullException(nameof(_headers));
        }

        var contentLength = _headers.SingleOrDefault(header => header.Key == "Content-Length");
        if (contentLength == null)
        {
            // if this header is not present we assume there is no entity body
            return 0;
        }

        if (!int.TryParse(contentLength.Value, out var length))
        {
            throw new ArgumentException("Content-Length header must be of type integer");
        }
        
        return length;
    }
}