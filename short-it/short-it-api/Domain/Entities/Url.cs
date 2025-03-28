using Domain.Abstractions;

namespace Domain.Entities;

public sealed class Url : BaseEntity
{
    public int UrlId { get; set; }
    public required string ShortUrl { get; set; }
    public required string LongUrl { get; set; }
    public ICollection<UrlMetric>? Metrics { get; set; }
}