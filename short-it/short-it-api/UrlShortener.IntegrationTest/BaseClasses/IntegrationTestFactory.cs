using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Contexts;

namespace UrlShortener.IntegrationTest.BaseClasses;

[ExcludeFromCodeCoverage]
public class IntegrationTestFactory<TStartup> : WebApplicationFactory<TStartup>
    where TStartup : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // Replace the production DbContext registration with in-memory DbContext
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<UrlDbContext>)
            );

            if (null != descriptor)
            {
                services.Remove(descriptor);
            }

            services.AddDbContext<UrlDbContext>(options =>
            {
                options.UseInMemoryDatabase("TestDatabase");
            });

            // Build the service provider and create a scope to resolve the seed data initializer
            using var scope = services.BuildServiceProvider().CreateScope();
            var serviceProvider = scope.ServiceProvider;
            var dbContext = serviceProvider.GetRequiredService<UrlDbContext>();

            // Ensure the database is deleted first for each integration test session
            dbContext.Database.EnsureDeleted();
                
            // Ensure the database is created and apply migrations
            dbContext.Database.EnsureCreated();

            // Seed the database with data
            SeedData.Initialize(dbContext);
        });
    }
}