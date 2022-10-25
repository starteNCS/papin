namespace Papin.Http;

public record HttpHeader
{
    public string Key { get; init; }
    public string Value { get; init; }
}