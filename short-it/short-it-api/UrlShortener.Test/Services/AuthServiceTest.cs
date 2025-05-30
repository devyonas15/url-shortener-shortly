using System.Security.Authentication;
using Application.Commons.DTO;
using Application.Exceptions;
using Application.Features.Login;
using AutoFixture;
using Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Moq;
using Persistence.Models;
using UrlShortener.Test.Abstractions;
using Xunit;

namespace UrlShortener.Test.Services;

public sealed class AuthServiceTest : TestFixture<AuthService>
{
    // Pretend the hash is correct according to PBKDF2
    private const string Password = "secret";
    private const string HashedPassword = "BDAMp8aA==";
    private static readonly Mock<UserManager<ApplicationUser>> UserManager = GetMockUserManager();
    private readonly AuthService _authService = new(UserManager.Object, Logger.Object, Mapper.Object);

    [Fact]
    public async Task GivenEmailNotFound_WhenSignInAsync_ThenThrowNotFoundException()
    {
        var request = Fixture.Build<LoginCommand>()
            .With(x => x.Email, "test@mailinator.com")
            .Create();
        UserManager.Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync((ApplicationUser?)null);

        await Assert.ThrowsAsync<NotFoundException>(async () =>
            await _authService.SignInAsync(request, CancellationToken.None));
    }

    [Fact]
    public async Task GivenPasswordNotMatches_WhenSignInAsync_ThenThrowInvalidCredentialException()
    {
        var user = Fixture.Build<ApplicationUser>()
            .With(x => x.PasswordHash, HashedPassword)
            .Create();
        var request = Fixture.Build<LoginCommand>()
            .With(x => x.Password, Password)
            .Create();

        UserManager.Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync(user);
        UserManager.Setup(x => x.CheckPasswordAsync(user, request.Password))
            .ReturnsAsync(false);

        await Assert.ThrowsAsync<InvalidCredentialException>(async () =>
            await _authService.SignInAsync(request, CancellationToken.None));
    }

    [Fact]
    public async Task GivenEmailFound_AndPasswordMatches_WhenSignInAsync_ThenSuccessfullySignIn()
    {
        var user = Fixture.Build<ApplicationUser>()
            .With(x => x.PasswordHash, HashedPassword)
            .Create();
        var request = Fixture.Build<LoginCommand>()
            .With(x => x.Password, Password)
            .Create();
        var response = Fixture.Build<LoginResponse>()
            .With(x => x.Email, user.Email)
            .Create();

        UserManager.Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync(user);
        UserManager.Setup(x => x.CheckPasswordAsync(user, request.Password))
            .ReturnsAsync(true);
        Mapper.Setup(x => x.Map<LoginResponse>(user))
            .Returns(response);

        var result = await _authService.SignInAsync(request, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(user.Email, result.Email);
    }

    // Need multiple null parameters as a dummy to mock the UserManager
    private static Mock<UserManager<ApplicationUser>> GetMockUserManager()
    {
        var store = new Mock<IUserStore<ApplicationUser>>();
        var userManager = new Mock<UserManager<ApplicationUser>>(
            store.Object, null!, null!, null!, null!, null!, null!, null!, null!);

        return userManager;
    }
}