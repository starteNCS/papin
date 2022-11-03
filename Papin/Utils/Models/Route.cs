using HttpMethod = Papin.Http.Request.HttpMethod;

namespace Papin.Utils.Models;

public sealed record Route(HttpMethod Method, string Uri, Action Handler);
