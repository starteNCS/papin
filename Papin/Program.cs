using Papin.Http.Response;
using Papin.Utils;
using Papin.WebHost;
using HttpMethod = Papin.Http.Request.HttpMethod;

var builder = new WebHostBuilder();

builder.AddRoute(HttpMethod.GET, "/", () =>
{
    Logger.WriteInfo("Hallo");

    return new HttpResponseBuilder()
        .SetStatus(HttpStatus.Ok)
        .Build();
});

await builder.Build().Start();