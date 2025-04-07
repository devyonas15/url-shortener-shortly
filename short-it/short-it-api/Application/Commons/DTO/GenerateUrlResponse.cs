namespace Application.Commons.DTO;

public sealed class GenerateUrlResponse
{
    public bool Success { get; set; }
    public int Id { get; set; }
    public string ShortUrl { get; set; } = string.Empty;
}