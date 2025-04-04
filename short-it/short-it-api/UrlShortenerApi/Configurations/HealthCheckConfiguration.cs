using System.Diagnostics.CodeAnalysis;
using Domain.Configurations;
using Microsoft.Extensions.Options;

namespace UrlShortenerApi.Configurations;

[ExcludeFromCodeCoverage]
public static class HealthCheckConfiguration
{
    public static IServiceCollection AddHealthCheckConfiguration(this IServiceCollection services)
    {
        return services.AddHealthMetrics();
    }

    // Add healthcheck service
    private static IServiceCollection AddHealthMetrics(this IServiceCollection services)
    {
        var urlShortenerDbOption = services.BuildServiceProvider()
            .GetRequiredService<IOptions<UrlShortenerDbOptions>>().Value;

        services.AddHealthChecks().AddSqlServer(urlShortenerDbOption.ConnectionString);
        
        return services;
    }
}