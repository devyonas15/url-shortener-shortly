using System.Text;
using Domain.Configurations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace UrlShortenerApi.Configurations;

public static class AuthConfiguration
{
    public static IServiceCollection AddAuthConfiguration(this IServiceCollection services)
    {
        var jwtTokenOptions = services.BuildServiceProvider()
            .GetRequiredService<IOptions<JwtTokenOptions>>().Value;

        services.AddAuthorization();
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(x =>
            {
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtTokenOptions.IssuerKey)),
                    ValidIssuer = jwtTokenOptions.Issuer,
                    ValidAudience = jwtTokenOptions.Audience,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                };
            });

        return services;
    }
}