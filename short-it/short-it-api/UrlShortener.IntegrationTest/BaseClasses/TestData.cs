using System.Diagnostics.CodeAnalysis;
using Domain.Constants;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Persistence.Contexts;
using Persistence.Models;
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

    public static void InitializeIdentity(AuthDbContext authContext)
    {
        if (authContext.Users.Any()) return;
        
        // Create password hasher
        var passwordHasher = new PasswordHasher<ApplicationUser>();
            
        // Create test user
        var testUser = new ApplicationUser
        {
            Id = Guid.NewGuid()
                .ToString(),
            UserName = "testtest@mailinator.com",
            Email = "testtest@mailinator.com",
            NormalizedUserName = "TESTTEST@MAILINATOR.COM",
            NormalizedEmail = "TESTTEST@MAILINATOR.COM",
            EmailConfirmed = true,
            SecurityStamp = Guid.NewGuid()
                .ToString(),
            FirstName = "test",
            LastName = "test"
        };
            
        // Hash the password
        testUser.PasswordHash = passwordHasher.HashPassword(testUser, "Test123!");
            
        // Add user to database
        authContext.Users.Add(testUser);

    }
}
