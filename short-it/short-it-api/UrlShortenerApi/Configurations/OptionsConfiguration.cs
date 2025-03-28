using System.Diagnostics.CodeAnalysis;
using Domain.Configurations;

namespace UrlShortenerApi.Configurations;

[ExcludeFromCodeCoverage]
public static class OptionsConfiguration
{
    public static IServiceCollection AddOptionsConfiguration(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddOptions<UrlShortenerDbOptions>()
            .Bind(configuration.GetSection(nameof(UrlShortenerDbOptions)));

        return services;
    }
}
