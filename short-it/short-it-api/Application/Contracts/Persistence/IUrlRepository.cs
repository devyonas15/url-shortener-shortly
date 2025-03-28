using Domain.Entities;

namespace Application.Contracts.Persistence;

public interface IUrlRepository : IBaseRepository<Url>
{
    public Task<Url?> GetByLongUrlAsync(string longUrl, CancellationToken cancellationToken = default);
    
    public Task<Url?> GetByShortUrlAsync(string shortUrl, CancellationToken cancellationToken = default);
}