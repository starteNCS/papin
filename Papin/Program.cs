using Papin.Utils;
using Papin.WebHost;
using HttpMethod = Papin.Http.Request.HttpMethod;

var builder = new WebHostBuilder();

builder.AddRoute(HttpMethod.GET, "/", () =>
{
    Logger.WriteInfo("Hallo");
});

await builder.Build().Start();