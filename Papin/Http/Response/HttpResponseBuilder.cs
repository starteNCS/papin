using Papin.Http.Shared;

namespace Papin.Http.Response;

public class HttpResponseBuilder
{
    private const string? _version = HttpVersion.HTTP1_1;
    private int? _statusCode;

    public HttpResponseBuilder SetStatus(int statusCode)
    {
        _statusCode = statusCode;
        return this;
    }

    public HttpResponse Build()
    {
        if (_version == null || _statusCode == null)
        {
            throw new ArgumentException("All values for statusline must be set");
        }

        return new HttpResponse
        {
            Version = _version!,
            StatusCode = _statusCode.Value
        };
    }
    
}