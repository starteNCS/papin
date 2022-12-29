using Papin.Http.Request;
using Papin.Http.Response;
using HttpMethod = Papin.Http.Request.HttpMethod;

namespace Papin.Utils.Models;

public sealed record Route(HttpMethod Method, string Uri, Func<HttpRequest, HttpResponse> Handler);
