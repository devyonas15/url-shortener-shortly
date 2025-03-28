using Domain.Abstractions;

namespace Domain.Entities;

public class UrlMetric : BaseEntity
{
    public int UrlMetricId { get; set; }
    
    public required int UrlId { get; set; }
    
    public required string AccessorIp { get; set; }
    
    public required string UserAgent { get; set; }
    
    public Url Url { get; set; }
}