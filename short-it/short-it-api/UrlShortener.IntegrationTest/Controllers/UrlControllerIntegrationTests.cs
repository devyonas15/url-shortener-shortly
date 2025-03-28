using System.Net;
using System.Net.Http.Json;
using Application.Features.GenerateUrl;
using UrlShortener.IntegrationTest.BaseClasses;
using UrlShortener.IntegrationTest.Constants;
using Xunit;

namespace UrlShortener.IntegrationTest.Controllers;

public sealed class UrlControllerIntegrationTests : IClassFixture<IntegrationTestFactory<Program>>
{
    private readonly HttpClient _client;

    public UrlControllerIntegrationTests(IntegrationTestFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task When_GetAllUrls_Endpoint_Is_Triggered_Then_Return_Ok()
    {
        // Act
        var response = await _client.GetAsync("/api/url/all");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
    
    [Fact]
    public async Task When_GetUrlById_Endpoint_Is_Triggered_Then_Return_Ok()
    {
        // Arrange
        var urlId = TestConstants.UrlId1.ToString();
    
        // Act
        var response = await _client.GetAsync($"/api/url/{urlId}");
    
        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
    
    [Fact]
    public async Task Given_IdNotExist_When_GetUrlByIdEndpoint_Then_Return_NotFound()
    {
        // Act
        var response = await _client.GetAsync($"/api/url/2");
    
        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
    
    [Fact]
    public async Task When_GenerateUrl_Endpoint_Is_Called_Then_Return_Created()
    {
        // Act
        var response = await _client.PostAsJsonAsync("/api/Url", new GenerateUrlCommand
        {
            LongUrl = "http://www.test.com"
        });
    
        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }
}