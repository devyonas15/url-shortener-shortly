using Domain.Abstractions;

namespace Domain.Entities;

public sealed class Url : BaseEntity
{
    public int UrlId { get; set; }
    public string UserId { get; set; } = string.Empty;
    public required string ShortUrl { get; set; } = string.Empty;
    public required string LongUrl { get; set; } = string.Empty;
    public ICollection<UrlMetric>? Metrics { get; set; }
}