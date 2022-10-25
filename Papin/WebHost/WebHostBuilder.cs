namespace Papin.WebHost;

public class WebHostBuilder
{
    private Dictionary<string, Action> Routes { get; set; }

    public WebHostBuilder()
    {
        Routes = new Dictionary<string, Action>();
    }

    public void AddRoute(string route, Action handler)
    {
        if (!Routes.TryAdd(route, handler))
        {
            throw new ArgumentException($"Could not add '{route}' to routes");
        }
    }

    public WebHost Build()
    {
        return new WebHost(Routes);
    }
}