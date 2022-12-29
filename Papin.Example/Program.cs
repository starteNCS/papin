using Papin.Example;
using Papin.Http.Response;
using Papin.WebHost;
using HttpMethod = Papin.Http.Request.HttpMethod;

var builder = new WebHostBuilder();

builder.AddRoute(HttpMethod.GET, "/", (_) =>
{
    HttpResponse httpResponse = new HttpResponseBuilder()
        .SetStatus(HttpStatus.Ok)
        .SetBody(new ExampleResponse
        {
            Name = "Philipp Honsel",
            Age = 22,
            NewsletterSubscribed = true
        })
        .Build();
    return Task.FromResult(httpResponse);
});

builder.AddRoute(HttpMethod.POST, "/echo", (request) => new HttpResponseBuilder()
    .SetStatus(HttpStatus.Ok)
    .SetBody(request.Body)
    .Build());

await builder.Build().Start();
