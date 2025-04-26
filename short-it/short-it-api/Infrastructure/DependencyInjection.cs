using System.Diagnostics.CodeAnalysis;
using Application.Contracts.Infrastructure;
using FluentValidation;
using Infrastructure.Services;
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
            .AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<AuthDbContext>()
            .AddDefaultTokenProviders();
        
        services.AddScoped<IAuthService, AuthService>();
        
        return services;
    }
}