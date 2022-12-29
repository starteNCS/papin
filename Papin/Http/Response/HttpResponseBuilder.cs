using Papin.Http.Shared;

namespace Papin.Http.Response;

/// <summary>
/// Used to build http responses reliably
/// </summary>
public class HttpResponseBuilder
{
    private const string? _version = HttpVersion.HTTP1_1;
    private int? _statusCode;
    private object? _body;
    private readonly Dictionary<string, string> _headers = new();

    /// <summary>
    /// Sets the http status code for the current response
    /// </summary>
    /// <param name="statusCode">Http status code</param>
    /// <returns>The adjusted response Builder</returns>
    public HttpResponseBuilder SetStatus(int statusCode)
    {
        _statusCode = statusCode;
        return this;
    }

    /// <summary>
    /// Adds one http header per call
    /// </summary>
    /// <param name="header">Name of the header</param>
    /// <param name="value">Value of the header</param>
    /// <returns>The adjusted response builder</returns>
    public HttpResponseBuilder AddHeader(string header, string value)
    {
        _headers.Add(header, value);
        return this;
    }

    /// <summary>
    /// Sets the http body for the current response
    /// </summary>
    /// <param name="body">Body</param>
    /// <returns>The adjusted response builder</returns>
    public HttpResponseBuilder SetBody(object body)
    {
        _body = body;
        return this;
    }

    /// <summary>
    /// Builds the response using all the provided values
    /// </summary>
    /// <returns>A valid http response</returns>
    /// <exception cref="ArgumentException">
    /// - StatusLine is invalid (code not set)
    /// </exception>
    public HttpResponse Build()
    {
        if (_version == null || _statusCode == null)
        {
            throw new ArgumentException("All values for statusline must be set");
        }

        return new HttpResponse
        {
            Version = _version!,
            StatusCode = _statusCode.Value,
            Headers = _headers,
            Body = _body
        };
    }
}
