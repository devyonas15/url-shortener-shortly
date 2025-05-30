using Application.Contracts.Persistence;
using Application.Exceptions;
using Application.Modules.Url.RedirectUrl;
using AutoFixture;
using Moq;
using UrlShortener.Test.Abstractions;
using Xunit;
using UrlEntity = Domain.Entities.Url;

namespace UrlShortener.Test.Applications.Modules.Url.RedirectUrl;

public sealed class RedirectUrlQueryHandlerTest : TestFixture<RedirectUrlQueryHandler>
{
    private static readonly Mock<IUrlRepository> UrlRepository = new();
    private readonly RedirectUrlQueryHandler _handler = new(Mapper.Object, Logger.Object, UrlRepository.Object);

    [Fact]
    public async Task GivenInvalidBase64Code_WhenRedirectUrlQuery_ThenShouldThrowBadRequestException()
    {
        var query = Fixture.Build<RedirectUrlQuery>()
            .With(x => x.Base64Code, string.Empty)
            .Create();
        
        await Assert.ThrowsAsync<BadRequestException>(async () => await _handler.Handle(query, CancellationToken.None));
    }

    [Fact]
    public async Task GivenValidBase64Code_WhenRedirectUrlQuery_AndMatchingUrlNotFound_ThenShouldThrowNotFoundException()
    {
        var query = Fixture.Build<RedirectUrlQuery>()
            .With(x => x.Base64Code, "qaW175Y")
            .Create();
        
        UrlRepository.Setup(x => x.GetByShortUrlAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((UrlEntity?)null);
        
        await Assert.ThrowsAsync<NotFoundException>(async () => await _handler.Handle(query, CancellationToken.None));
    }

    [Fact]
    public async Task GivenValidBase64Code_WhenRedirectUrlQuery_AndMatchingUrlFound_ThenShouldReturnCorrectUrl()
    {
        const string expectedLongUrl = "https://test.example.com/";
        var query = Fixture.Build<RedirectUrlQuery>()
            .With(x => x.Base64Code, "123aqwqy")
            .Create();
        var mockUrl = Fixture.Build<UrlEntity>()
            .With(u => u.LongUrl, expectedLongUrl)
            .Create();
        
        UrlRepository.Setup(x => x.GetByShortUrlAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(mockUrl);
        
        var response = await _handler.Handle(query, CancellationToken.None);
        
        Assert.NotEmpty(response);
    }
}