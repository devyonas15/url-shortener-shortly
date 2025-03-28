using Application.Contracts.Persistence;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;

namespace Persistence.Repositories;

public sealed class UrlRepository : BaseRepository<Url>, IUrlRepository
{
    public UrlRepository(UrlDbContext context) : base(context)
    {
    }

    public async Task<Url?> GetByLongUrlAsync(string longUrl, CancellationToken cancellationToken = default)
    {
        var url = Context.Urls.Where(u => u.LongUrl == longUrl);
        
        return await url.AsNoTracking().FirstOrDefaultAsync(cancellationToken: cancellationToken);
    }

    public async Task<Url?> GetByShortUrlAsync(string shortUrl, CancellationToken cancellationToken = default)
    {
       var url = Context.Urls.Where(u => u.ShortUrl == shortUrl);
       
       return await url.AsNoTracking().FirstOrDefaultAsync(cancellationToken: cancellationToken);
    }
}