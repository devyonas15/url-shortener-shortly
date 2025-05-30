using System.Net;
using System.Net.Http.Json;
using Application.Features.Login;
using UrlShortener.IntegrationTest.BaseClasses;
using Xunit;

namespace UrlShortener.IntegrationTest.Controllers;

public sealed class AuthControllerIntegrationTest : IClassFixture<IntegrationTestFactory<Program>>
{
    private readonly HttpClient _client;

    public AuthControllerIntegrationTest(IntegrationTestFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }
    
    [Fact]
    public async Task When_Login_Endpoint_Is_Called_Then_Return_Ok()
    {
        var response = await _client.PostAsJsonAsync("/api/auth/login", new LoginCommand
        {
            Email = "testtest@mailinator.com",
            Password = "Test123!"
        });
        
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}