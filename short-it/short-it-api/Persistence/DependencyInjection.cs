using System.Diagnostics.CodeAnalysis;
using Application.Contracts.Persistence;
using Domain.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Persistence.Contexts;
using Persistence.Repositories;

namespace Persistence;

[ExcludeFromCodeCoverage]
public static class DependencyInjection
{
    public static IServiceCollection AddPersistenceServices(this IServiceCollection services)
    {
        return services.AddUrlShortenerDb()
        .AddRepositories();
    }

    private static IServiceCollection AddUrlShortenerDb(this IServiceCollection services)
    {
        void OptionsAction(IServiceProvider sp, DbContextOptionsBuilder options)
        {
            var urlShortenerDbOption = services.BuildServiceProvider()
                .GetRequiredService<IOptions<UrlShortenerDbOptions>>().Value;

            options.UseSqlServer(urlShortenerDbOption.ConnectionString);
        }

        services.AddDbContext<UrlDbContext>(OptionsAction);
        services.AddDbContext<AuthDbContext>(OptionsAction);

        return services;
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
        services.AddScoped<IUrlRepository, UrlRepository>();
    
        return services;
    }
}