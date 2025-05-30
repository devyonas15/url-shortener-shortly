using Application.Contracts.Infrastructure;
using Application.Exceptions;
using Application.Modules.Auth.Login;
using AutoFixture;
using Moq;
using UrlShortener.Test.Abstractions;
using Xunit;

namespace UrlShortener.Test.Applications.Modules.Auth.Login;

public sealed class LoginCommandHandlerTest : TestFixture<LoginCommandHandler>
{
    private static readonly Mock<IAuthService> AuthService = new();
    private static readonly Mock<ITokenService> TokenService = new();

    private readonly LoginCommandHandler _handler = new(Mapper.Object, Logger.Object, AuthService.Object,
        TokenService.Object);

    [Fact]
    public async Task GivenInvalidRequest_WhenHandle_ThenThrowBadRequestException()
    {
        var request = Fixture.Build<LoginCommand>()
            .With(x => x.Email, "")
            .Create();

        await Assert.ThrowsAsync<BadRequestException>(async () =>
            await _handler.Handle(request, CancellationToken.None));
    }

    [Fact]
    public async Task GivenValidRequest_WhenHandle_ThenReturnLoginResponse()
    {
        const string expectedToken = "accesstoken";
        var request = Fixture.Build<LoginCommand>()
            .With(x => x.Email, "test@mailinator.com")
            .With(x => x.Password, "Test@1234")
            .Create();
        var loginResponse = Fixture.Build<LoginResponse>()
            .With(x => x.LoginId, "1")
            .With(x => x.Email, request.Email)
            .Create();

        AuthService.Setup(x => x.SignInAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(loginResponse);
        TokenService.Setup(x => x.GenerateJwtToken(loginResponse.LoginId, loginResponse.Email))
            .Returns(expectedToken); // Pretend this is a valid JWT token for testing

        var response = await _handler.Handle(request, CancellationToken.None);

        Assert.NotNull(response);
        Assert.Equal(loginResponse.LoginId, response.LoginId);
        Assert.Equal(loginResponse.Email, response.Email);
        Assert.Equal(expectedToken, response.AccessToken);
    }
}