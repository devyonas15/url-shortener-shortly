using System.IdentityModel.Tokens.Jwt;
using AutoFixture;
using Domain.Configurations;
using Infrastructure.Services.Auth;
using Microsoft.Extensions.Options;
using Moq;
using UrlShortener.Test.Abstractions;
using Xunit;

namespace UrlShortener.Test.Services;

public sealed class TokenServiceTest : TestFixture<TokenService>
{
    private const string UserId = "1";
    private const string Email = "test@mailinator.com";
    private static readonly Mock<IOptions<JwtTokenOptions>> MockJwtTokenOptions = new();
    private static TokenService _tokenService;

    [Fact]
    public void GivenValidUserIdAndEmail_WhenGenerateJwtToken_ThenReturnValidToken()
    {
        var tokenOptions = Fixture.Build<JwtTokenOptions>()
            .With(x => x.Issuer, "test")
            .With(x => x.Audience, "test")
            .With(x => x.IssuerKey,
                "VtvZiGMu8CiaxOEUxSv+LT7ZCALavQkRqkm7XF5htAU=") // Only a random test key - nothing important
            .With(x => x.ExpiresInMinutes, 60)
            .Create();
        MockJwtTokenOptions.Setup(x => x.Value)
            .Returns(tokenOptions);
        _tokenService = new TokenService(Logger.Object, Mapper.Object, MockJwtTokenOptions.Object);

        var tokenHandler = new JwtSecurityTokenHandler();

        var result = _tokenService.GenerateJwtToken(UserId, Email);

        // Assert Not Empty
        Assert.NotEmpty(result);

        // Assert that it is JWT token
        Assert.True(tokenHandler.CanReadToken(result));

        // Assert the body of JWT token
        var token = tokenHandler.ReadJwtToken(result);
        Assert.Equal(MockJwtTokenOptions.Object.Value.Issuer, token.Issuer);
        Assert.Equal(UserId, token.Claims.First(x => x.Type == "sub").Value);
        Assert.Equal(Email, token.Claims.First(x => x.Type == "email").Value);
    }

    [Fact]
    public void GivenInvalidIssuerKey_WhenGenerateJwtToken_ThenThrowException()
    {
        var tokenOptions = Fixture.Build<JwtTokenOptions>()
            .With(x => x.Issuer, "test")
            .With(x => x.Audience, "test")
            .With(x => x.IssuerKey, "test") // invalid 32 byte - base64 key
            .With(x => x.ExpiresInMinutes, 30)
            .Create();
        MockJwtTokenOptions.Setup(x => x.Value)
            .Returns(tokenOptions);
        _tokenService = new TokenService(Logger.Object, Mapper.Object, MockJwtTokenOptions.Object);

        Assert.Throws<Exception>(() => _tokenService.GenerateJwtToken(UserId, Email));
    }
}