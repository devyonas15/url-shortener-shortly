using Domain.Extensions;
using Xunit;

namespace UrlShortener.Test.Domain.Extensions;

public sealed class UrlExtensionsTest
{
    [Fact]
    public void GivenValidLongUrl_WhenToBase64Prefix_ThenReturnPrefix()
    {
        const string url = "www.example.com";
        Assert.Equal("gPwPuSZ", url.ToBase64Prefix());
    }
}