using System.Diagnostics.CodeAnalysis;
using System.Text;
using Domain.Configurations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Persistence.Contexts;

namespace UrlShortener.IntegrationTest.BaseClasses;

[ExcludeFromCodeCoverage]
public class IntegrationTestFactory<TStartup> : WebApplicationFactory<TStartup>
    where TStartup : class
{
    // Test JWT constants
    private const string TestIssuerKey = "D9wQmzK7kA+xuEyN2mM5ReNdpq7B8uDh2U6ehWJKk7c=";
    private const string TestIssuer = "test-issuer";
    private const string TestAudience = "test-audience";
    
    protected override IHost CreateHost(IHostBuilder builder)
    {
        // Override configuration values
        builder.ConfigureHostConfiguration(config =>
        {
            // Create a dictionary with our test JWT settings
            var testSettings = new Dictionary<string, string>
            {
                [$"JwtTokenOptions:IssuerKey"] = TestIssuerKey,
                [$"JwtTokenOptions:Issuer"] = TestIssuer,
                [$"JwtTokenOptions:Audience"] = TestAudience,
                [$"JwtTokenOptions:ExpiresInMinutes"] = "60"
            };

            // Add these settings to the configuration
            config.AddInMemoryCollection(testSettings);
        });
        
        // First, create the host so we can use its services
        var host = base.CreateHost(builder);
        
        // Replace the JwtBearerHandler's TokenValidationParameters with our test values
        using (var scope = host.Services.CreateScope())
        {
            var jwtBearerHandler = scope.ServiceProvider
                .GetRequiredService<IAuthenticationHandler>();
            
            // Attempt to use reflection to access and modify TokenValidationParameters
            var tokenValidationParametersField = jwtBearerHandler.GetType()
                .GetField("_tokenValidationParameters", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            
            if (tokenValidationParametersField != null)
            {
                var parameters = new TokenValidationParameters
                {
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(TestIssuerKey)),
                    ValidIssuer = TestIssuer,
                    ValidAudience = TestAudience,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ValidateIssuer = true,
                    ValidateAudience = true
                };
                
                tokenValidationParametersField.SetValue(jwtBearerHandler, parameters);
            }
        }
        
        return host;
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // Replace DbContexts with in-memory versions
            ReplaceDbContexts(services);
            
            // Ensure our JwtTokenOptions are used instead of the ones from configuration
            services.AddSingleton(new JwtTokenOptions
            {
                IssuerKey = TestIssuerKey,
                Issuer = TestIssuer,
                Audience = TestAudience,
                ExpiresInMinutes = 60
            });
            
            // Modify JwtBearerOptions
            services.PostConfigureAll<JwtBearerOptions>(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(TestIssuerKey)),
                    ValidIssuer = TestIssuer,
                    ValidAudience = TestAudience,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ValidateIssuer = true,
                    ValidateAudience = true
                };
            });
            
            // Seed test data
            SeedTestData(services);
        });
    }

    private void ReplaceDbContexts(IServiceCollection services)
    {
        // Remove existing DB context registrations
        var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<UrlDbContext>));
        if (descriptor != null)
        {
            services.Remove(descriptor);
        }

        var authDbContextDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<AuthDbContext>));
        if (authDbContextDescriptor != null)
        {
            services.Remove(authDbContextDescriptor);
        }

        // Add in-memory DB contexts
        services.AddDbContext<UrlDbContext>(options => options.UseInMemoryDatabase("TestDatabase"));
        services.AddDbContext<AuthDbContext>(options => options.UseInMemoryDatabase("TestDatabase"));
    }

    private void SeedTestData(IServiceCollection services)
    {
        // Create scope and seed data
        using var scope = services.BuildServiceProvider(validateScopes: false).CreateScope();
        var serviceProvider = scope.ServiceProvider;

        // Get DB Contexts
        var dbContext = serviceProvider.GetRequiredService<UrlDbContext>();
        var authDbContext = serviceProvider.GetRequiredService<AuthDbContext>();

        // Reset databases
        dbContext.Database.EnsureDeleted();
        authDbContext.Database.EnsureDeleted();
        dbContext.Database.EnsureCreated();
        authDbContext.Database.EnsureCreated();

        // Seed the database with data
        SeedData.Initialize(dbContext);
        SeedData.InitializeIdentity(authDbContext);
    }
}





