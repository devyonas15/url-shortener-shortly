using Application.Modules.Auth.Login;
using AutoFixture;
using UrlShortener.Test.Abstractions;
using Xunit;

namespace UrlShortener.Test.Applications.Modules.Auth.Login;

public sealed class LoginCommandValidatorTest : TestFixture<LoginCommandValidator>
{
    private readonly LoginCommandValidator _validator = new();

    [Theory]
    [InlineData("")]
    [InlineData("test.c")]
    public void GivenInvalidEmail_WhenLoginCommandValidator_ThenShouldBeInvalid(string email)
    {
        var request = Fixture.Build<LoginCommand>()
            .With(x => x.Email, email)
            .Create();

        var result = _validator.Validate(request);

        Assert.False(result.IsValid);
    }

    [Theory]
    [InlineData("")]
    [InlineData("3qtnh*")]
    [InlineData("asdasdasdasd")]
    public void GivenInvalidPassword_WhenLoginCommandValidator_ThenShouldBeInvalid(string password)
    {
        var request = Fixture.Build<LoginCommand>()
            .With(x => x.Password, password)
            .Create();

        var result = _validator.Validate(request);

        Assert.False(result.IsValid);
    }

    [Fact]
    public void GivenValidParameters_WhenLoginCommandValidator_ThenShouldPass()
    {
        var request = Fixture.Build<LoginCommand>()
            .With(x => x.Email, "test@mailinator.com")
            .With(x => x.Password, "Test@1234")
            .Create();

        var result = _validator.Validate(request);

        Assert.True(result.IsValid);
    }
}