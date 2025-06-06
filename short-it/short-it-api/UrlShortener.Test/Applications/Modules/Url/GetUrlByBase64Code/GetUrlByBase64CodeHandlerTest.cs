using Application.Commons.DTO;
using Application.Contracts.Persistence;
using Application.Exceptions;
using Application.Modules.Url.GetUrlByBase64Code;
using AutoFixture;
using Moq;
using UrlShortener.Test.Abstractions;
using Xunit;
using UrlEntity = Domain.Entities.Url;

namespace UrlShortener.Test.Applications.Modules.Url.GetUrlByBase64Code;

public sealed class GetUrlByBase64CodeHandlerTest : TestFixture<GetUrlByBase64CodeQueryHandler>
{
    private static readonly Mock<IUrlRepository> UrlRepository = new();
    private readonly GetUrlByBase64CodeQueryHandler _handler = new(UrlRepository.Object, Mapper.Object, Logger.Object);


    [Fact]
    public async Task GivenInvalidRequest_WhenGetUrlByBase64Code_ThenThrowsBadRequestException()
    {
        var query = Fixture.Build<GetUrlByBase64CodeQuery>()
            .With(x => x.Base64Code, "")
            .Create();

        await Assert.ThrowsAsync<BadRequestException>(async () => await _handler.Handle(query, CancellationToken.None));
    }

    [Fact]
    public async Task GivenValidRequest_WhenDataNotFound_ThenGetUrlByBase64CodeThrowsNotFoundException()
    {
        var query = Fixture.Build<GetUrlByBase64CodeQuery>()
            .With(x => x.Base64Code, "asfdag3")
            .Create();

        UrlRepository.Setup(x => x.GetByShortUrlAsync(It.IsAny<string>(), CancellationToken.None))
            .ReturnsAsync((UrlEntity?)null);

        await Assert.ThrowsAsync<NotFoundException>(async () => await _handler.Handle(query, CancellationToken.None));
    }

    [Fact]
    public async Task GivenValidRequest_WhenDataIsFound_ThenGetUrlByBase64CodeReturnsUrl()
    {
        var query = Fixture.Build<GetUrlByBase64CodeQuery>()
            .With(x => x.Base64Code, "asfdag3")
            .Create();

        UrlRepository.Setup(x => x.GetByShortUrlAsync(It.IsAny<string>(), CancellationToken.None))
            .ReturnsAsync(Fixture.Create<UrlEntity>());

        Mapper.Setup(x => x.Map<UrlResponse>(It.IsAny<UrlEntity>()))
            .Returns(Fixture.Create<UrlResponse>());

        var response = await _handler.Handle(query, CancellationToken.None);

        Assert.NotNull(response);
    }
}