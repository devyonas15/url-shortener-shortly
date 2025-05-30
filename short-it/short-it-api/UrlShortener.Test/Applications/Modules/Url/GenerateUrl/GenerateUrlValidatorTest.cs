using Application.Modules.Url.GenerateUrl;
using AutoFixture;
using UrlShortener.Test.Abstractions;
using Xunit;

namespace UrlShortener.Test.Applications.Modules.Url.GenerateUrl;

public sealed class GenerateUrlValidatorTest : TestFixture<GenerateUrlValidatorTest>
{
    private readonly GenerateUrlCommandValidator _validator = new();

    [Fact]
    public void GivenEmptyUrl_ThenReturnFalse()
    {
        var request = Fixture.Build<GenerateUrlCommand>()
            .With(x => x.LongUrl, string.Empty)
            .Create();

        var result = _validator.Validate(request);

        Assert.False(result.IsValid);
    }

    [Fact]
    public void GivenInvalidUrl_ThenReturnFalse()
    {
        var request = Fixture.Build<GenerateUrlCommand>()
            .With(x => x.LongUrl, "asdasd")
            .Create();

        var result = _validator.Validate(request);

        Assert.False(result.IsValid);
    }

    [Theory]
    [InlineData("http://www.abcde.com")]
    [InlineData("https://www.test.com")]
    [InlineData("https://www.google.com")]
    public void GivenValidUrl_ThenReturnTrue(string url)
    {
        var request = Fixture.Build<GenerateUrlCommand>()
            .With(x => x.LongUrl, url)
            .Create();

        var result = _validator.Validate(request);

        Assert.True(result.IsValid);
    }
}