using System.Text;
using System.Text.Json;

namespace Papin.Http.Response;

public sealed record HttpResponse
{
    public string Version { get; set; } = string.Empty;
    public int StatusCode { get; set; }
    public Dictionary<string, string> Headers { get; set; } = new();
    public object? Body { get; set; }

    /// <summary>
    /// Creates the HTTP1.1 Version of the response string
    /// </summary>
    /// <returns>Response string</returns>
    public override string ToString()
    {
        var builder = new StringBuilder();

        // StatusLine
        builder.Append(Version);
        builder.Append(' ');
        builder.Append(StatusCode);
        builder.Append(' ');
        builder.Append(HttpStatus.ToReasonPhrase(StatusCode));
        builder.Append("\r\n");

        // Headers - Body dividing line
        builder.Append("\r\n");

        // Body
        if (Body != null)
        {
            builder.Append(JsonSerializer.Serialize(Body));
        }

        return builder.ToString();
    }

}
