using Application.Features.GetUrlByBase64Code;
using AutoFixture;
using UrlShortener.Test.Abstractions;
using Xunit;

namespace UrlShortener.Test.Applications.Features.GetUrlByBase64Code;

public sealed class GetUrlByBase64CodeValidatorTest : TestFixture<GetUrlByBase64CodeValidator>
{
    private readonly GetUrlByBase64CodeValidator _validator = new();

    [Fact]
    public void GivenInvalidRequest_WhenGetUrlByBase64Code_ThenShouldFail()
    {
        var query = Fixture.Build<GetUrlByBase64CodeQuery>()
            .With(x => x.Base64Code, string.Empty)
            .Create();

        var result = _validator.Validate(query);

        Assert.False(result.IsValid);
    }

    [Fact]
    public void GivenValidRequest_WhenGetUrlByBase64Code_ThenShouldPass()
    {
        var query = Fixture.Build<GetUrlByBase64CodeQuery>()
            .With(x => x.Base64Code, "ab3c0f3")
            .Create();

        var result = _validator.Validate(query);

        Assert.True(result.IsValid);
    }
}