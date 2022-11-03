namespace Papin.Http;

public record HttpHeader
{
    public string Key { get; init; } = string.Empty;
    public string Value { get; init; } = string.Empty;
}