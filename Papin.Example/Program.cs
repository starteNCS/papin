using Papin.Example;
using Papin.Http.Response;
using Papin.WebHost;
using HttpMethod = Papin.Http.Request.HttpMethod;

var builder = new WebHostBuilder();

builder.AddRoute(HttpMethod.GET, "/", () => new HttpResponseBuilder()
    .SetStatus(HttpStatus.Ok)
    .SetBody(new ExampleResponse
    {
        Name = "Philipp Honsel",
        Age = 22,
        NewsletterSubscribed = true
    })
    .Build());

await builder.Build().Start();
