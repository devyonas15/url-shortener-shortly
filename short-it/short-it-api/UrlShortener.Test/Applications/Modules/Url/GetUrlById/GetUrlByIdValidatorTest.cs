using Application.Modules.Url.GetUrlById;
using AutoFixture;
using UrlShortener.Test.Abstractions;
using Xunit;

namespace UrlShortener.Test.Applications.Modules.Url.GetUrlById;

public sealed class GetUrlByIdValidatorTest : TestFixture<GetUrlByIdQueryValidator>
{
    private readonly GetUrlByIdQueryValidator _validator = new();

    [Fact]
    public void GivenInvalidUrlId_WhenGetUrlByIdQuery_ThenShouldFail()
    {
        var query = Fixture.Build<GetUrlByIdQuery>()
            .With(x => x.UrlId, -1)
            .Create();

        var result = _validator.Validate(query);

        Assert.False(result.IsValid);
    }

    [Fact]
    public void GivenValidUrlId_WhenGetUrlByIdQuery_ThenShouldPass()
    {
        var query = Fixture.Build<GetUrlByIdQuery>()
            .With(x => x.UrlId, 1)
            .Create();

        var result = _validator.Validate(query);

        Assert.True(result.IsValid);
    }
}