using HttpMethod = Papin.Http.HttpMethod;

namespace Papin.Utils.Models;

public sealed record Route(HttpMethod Method, string Uri, Action Handler);
