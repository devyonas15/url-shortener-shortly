using System.Diagnostics.CodeAnalysis;
using Domain.Constants;
using Domain.Entities;
using Persistence.Contexts;
using UrlShortener.IntegrationTest.Constants;

namespace UrlShortener.IntegrationTest.BaseClasses;

[ExcludeFromCodeCoverage]
public static class SeedData
{
    public static void Initialize(UrlDbContext context)
    {
        if (context.Urls.Any()) return;
        context.Urls.AddRange(
            new Url
            {
                UrlId = TestConstants.UrlId1,
                ShortUrl = $"{UrlConstants.BaseShortUrl}{TestConstants.UrlShortPrefix1}",
                LongUrl = TestConstants.OriginalUrl1,
            }
        );

        context.SaveChanges();
    }
}
