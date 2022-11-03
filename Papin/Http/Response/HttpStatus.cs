namespace Papin.Http.Response;

public static class HttpStatus
{
    public const int Continue = 100;
    public const int SwitchingProtocols = 101;
    
    public const int Ok = 200;
    public const int Created = 201;
    public const int Accepted = 202;
    public const int NonAuthoritativeInformation = 203;
    public const int NoContent = 204;
    public const int ResetContent = 205;
    public const int PartialContent = 206;
    
    public const int MultipleChoices = 300;
    public const int MovedPermanently = 301;
    public const int Found = 302;
    public const int SeeOther = 303;
    public const int NotModified = 304;
    public const int UseProxy = 305;
    public const int TemporaryRedirect = 307;
    
    public const int BadRequest = 400;
    public const int Unauthorized = 401;
    public const int PaymentRequired = 402;
    public const int Forbidden = 403;
    public const int NotFound = 404;
    public const int MethodNotAllowed = 405;
    public const int NotAcceptable = 406;
    public const int ProxyAuthenticationRequired = 407;
    public const int RequestTimeOut = 408;
    public const int Conflict = 409;
    public const int Gone = 410;
    public const int LengthRequired = 411;
    public const int PreconditionFailed = 412;
    public const int RequestEntityTooLarge = 413;
    public const int RequestUriTooLarge = 414;
    public const int UnsupportedMediaType = 415;
    public const int RequestRangeNotSatisfiable = 416;
    public const int ExpectationFailed = 417;
    
    public const int InternalServerError = 500;
    public const int NotImplemented = 501;
    public const int BadGateway = 502;
    public const int ServiceUnavailable = 503;
    public const int GatewayTimeOut = 504;
    public const int HttpVersionNotSupported = 505;

    public static string ToReasonPhrase(int statusCode)
    {
        return statusCode switch
        {
            100 => "Continue",
            101 => "Switching Protocols",
            
            200 => "Ok",
            201 => "Created",
            202 => "Accepted",
            203 => "Non-Authoritative Information",
            204 => "No Content",
            205 => "Reset Content",
            206 => "Partial Content",
            
            300 => "Multiple Choices",
            301 => "Moved Permanently",
            302 => "Found",
            303 => "See Other",
            304 => "Not Modified",
            305 => "Use Proxy",
            307 => "Temporary Redirect",
            
            400 => "BadRequest",
            401 => "Unauthorized",
            402 => "Payment Required",
            403 => "Forbidden",
            404 => "NotFound",
            405 => "Method Not Allowed",
            406 => "Not Acceptable",
            407 => "Proxy Authentication Required",
            408 => "Request Time-out",
            409 => "Conflict",
            410 => "Gone",
            411 => "Length Required",
            412 => "Precondition Failed",
            413 => "Request Entity Too Large",
            414 => "Request-Uri Too Large",
            415 => "Unsupported Media Type",
            416 => "Request Range Not satisfiable",
            417 => "Expectation Failed",
            
            500 => "Internal Server Error",
            501 => "Not Implemented",
            502 => "Bad Gateway",
            503 => "Service Unavailable",
            504 => "Gateway Time-out",
            505 => "Http Version not supported",
            _ => throw new ArgumentOutOfRangeException(nameof(statusCode), statusCode, null)
        };
    }
}