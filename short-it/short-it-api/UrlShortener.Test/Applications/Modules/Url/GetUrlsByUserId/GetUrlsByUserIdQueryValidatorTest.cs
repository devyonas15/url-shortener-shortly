using Application.Modules.Url.GetUrlsByUserId;
using AutoFixture;
using UrlShortener.Test.Abstractions;
using Xunit;

namespace UrlShortener.Test.Applications.Modules.Url.GetUrlsByUserId;

public sealed class GetUrlsByUserIdQueryValidatorTest : TestFixture<GetUrlsByUserIdQueryValidator>
{
    private readonly GetUrlsByUserIdQueryValidator _validator = new();

    [Fact]
    public void GivenInvalidUrlId_WhenGetUrlByIdQuery_ThenShouldFail()
    {
        var query = Fixture.Build<GetUrlsByUserIdQuery>()
            .With(x => x.UserId, "")
            .Create();

        var result = _validator.Validate(query);

        Assert.False(result.IsValid);
    }

    [Fact]
    public void GivenValidUrlId_WhenGetUrlByIdQuery_ThenShouldPass()
    {
        var query = Fixture.Build<GetUrlsByUserIdQuery>()
            .With(x => x.UserId, "AAAA1111-AAA1-1111-AA1A-111111AA1111")
            .Create();

        var result = _validator.Validate(query);

        Assert.True(result.IsValid);
    }
}