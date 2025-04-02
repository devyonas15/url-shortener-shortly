using Domain.Helpers;
using Xunit;

namespace UrlShortener.Test.Domain.Helpers;

public sealed class UrlHelpersTest
{
    [Theory]
    [InlineData("http://www.abcde.com")]
    [InlineData("https://www.test.com")]
    [InlineData("https://www.google.com")]
    public void GivenValidUrl_ThenReturnTrue(string url)
    {
        var result = UrlHelpers.IsUrlValid(url);

        Assert.True(result);
    }
}