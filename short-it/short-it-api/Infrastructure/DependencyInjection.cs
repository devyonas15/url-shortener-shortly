using System.Diagnostics.CodeAnalysis;
using Application.Contracts.Infrastructure;
using FluentValidation;
using Infrastructure.Services.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Contexts;
using Persistence.Models;

namespace Infrastructure;

public static class DependencyInjection
{
    [ExcludeFromCodeCoverage]
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        return services.AddServices();
    }

    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        var assembly = typeof(DependencyInjection).Assembly;

        services
            .AddAutoMapper(assembly)
            .AddValidatorsFromAssembly(assembly)
            .AddIdentityCore<ApplicationUser>() //Needed for API only, since AddIdentity() is for the API + Blazor
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<AuthDbContext>()
            .AddDefaultTokenProviders();

        // Add services
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ITokenService, TokenService>();

        return services;
    }
}