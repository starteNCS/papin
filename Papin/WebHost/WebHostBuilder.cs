using Papin.Http.Request;
using Papin.Http.Response;
using Papin.Utils.Models;
using HttpMethod = Papin.Http.Request.HttpMethod;

namespace Papin.WebHost;

public class WebHostBuilder
{
    private readonly List<Route> _routes;

    public WebHostBuilder()
    {
        _routes = new List<Route>();
    }

    public void AddRoute(HttpMethod method, string route, Func<HttpRequest, HttpResponse> handler)
    {
        _routes.Add(new Route(method, route, req => Task.FromResult(handler(req))));
    }

    public void AddRoute(HttpMethod method, string route, Func<HttpRequest, Task<HttpResponse>> handler)
    {
        _routes.Add(new Route(method, route, handler));
    }

    public WebHost Build()
    {
        return new WebHost(_routes);
    }
}
