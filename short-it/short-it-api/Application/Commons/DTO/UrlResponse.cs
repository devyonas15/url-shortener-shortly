namespace Application.Commons.DTO;

public sealed class UrlResponse
{
    public int UrlId { get; set; }
    public string ShortUrl { get; set; } = string.Empty;
    public string OriginalUrl { get; set; } = string.Empty;
}