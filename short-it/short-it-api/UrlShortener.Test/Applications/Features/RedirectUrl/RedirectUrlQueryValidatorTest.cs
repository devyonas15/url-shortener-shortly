using Application.Features.RedirectUrl;
using AutoFixture;
using UrlShortener.Test.Abstractions;
using Xunit;

namespace UrlShortener.Test.Applications.Features.RedirectUrl;

public sealed class RedirectUrlQueryValidatorTest : TestFixture<RedirectUrlQueryValidator>
{
    private readonly RedirectUrlQueryValidator _validator = new();

    [Fact]
    public void GivenEmptyBase64Code_WhenRedirectUrlQuery_ThenShouldFail()
    {
        var query = Fixture.Build<RedirectUrlQuery>()
            .With(x => x.Base64Code, string.Empty)
            .Create();
        
        var result = _validator.Validate(query);
        
        Assert.False(result.IsValid);
    }
    
    [Fact]
    public void GivenValidBase64Code_WhenRedirectUrlQuery_ThenShouldPass()
    {
        var query = Fixture.Build<RedirectUrlQuery>()
            .With(x => x.Base64Code, "qA123wq")
            .Create();
        
        var result = _validator.Validate(query);
        
        Assert.True(result.IsValid);
    }
}