using System.Text;

namespace Papin.Http.Response;

public sealed record HttpResponse
{
    public string Version { get; set; } = string.Empty;
    public int StatusCode { get; set; }
    public Dictionary<string, string> Headers { get; set; } = new();

    public override string ToString()
    {
        var builder = new StringBuilder();

        // StatusLine
        builder.Append(Version);
        builder.Append(' ');
        builder.Append(StatusCode);
        builder.Append(' ');
        builder.Append(HttpStatus.ToReasonPhrase(StatusCode));
        builder.Append('\r', '\n');

        // Headers - Body 
        builder.Append('\r', '\n');
        
        return builder.ToString();
    }
    
}