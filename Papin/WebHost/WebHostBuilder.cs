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

    public void AddRoute(HttpMethod method, string route, Func<HttpResponse> handler)
    {
        _routes.Add(new Route(method, route, handler));
    }

    public WebHost Build()
    {
        return new WebHost(_routes);
    }
}