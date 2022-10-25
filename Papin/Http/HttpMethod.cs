// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global
namespace Papin.Http;

public enum HttpMethod
{
    GET,
    POST,
    PATCH,
    PUT,
    DELETE
}

public static class HttpMethodExtensions
{
    public static HttpMethod ToHttpMethod(this string method)
    {
        return method switch
        {
            "GET" => HttpMethod.GET,
            "POST" => HttpMethod.POST,
            "PATCH" => HttpMethod.PATCH,
            "PUT" => HttpMethod.PUT,
            "DELETE" => HttpMethod.DELETE,
            _ => throw new ArgumentException("Unsupported HTTP Method received")
        };
    }
}