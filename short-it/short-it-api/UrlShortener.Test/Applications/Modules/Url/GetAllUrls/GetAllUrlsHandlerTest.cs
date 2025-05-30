using Application.Commons.DTO;
using Application.Contracts.Persistence;
using Application.Exceptions;
using Application.Modules.Url.GetAllUrls;
using AutoFixture;
using Moq;
using UrlShortener.Test.Abstractions;
using Xunit;
using UrlEntity = Domain.Entities.Url;

namespace UrlShortener.Test.Applications.Modules.Url.GetAllUrls;

public sealed class GetAllUrlsHandlerTest : TestFixture<GetAllUrlsQueryHandler>
{
    private static readonly Mock<IUrlRepository> UrlRepository = new();
    private readonly GetAllUrlsQueryHandler _handler = new(UrlRepository.Object, Mapper.Object, Logger.Object);

    [Fact]
    public async Task GivenDataNotFound_ThenThrowNotFoundException()
    {
        var mockRequest = CreateMockRequest();

        UrlRepository.Setup(x => x.GetAllAsync(CancellationToken.None))
            .ReturnsAsync(new List<UrlEntity>());

        await Assert.ThrowsAsync<NotFoundException>(async () =>
            await _handler.Handle(mockRequest, It.IsAny<CancellationToken>()));
    }

    [Fact]
    public async Task GivenDataIsFound_ThenReturnUrls()
    {
        var mockRequest = CreateMockRequest();

        UrlRepository.Setup(x => x.GetAllAsync(CancellationToken.None))
            .ReturnsAsync(Fixture
                .CreateMany<UrlEntity>().ToList());

        Mapper.Setup(x => x.Map<IReadOnlyList<UrlResponse>>(It.IsAny<IReadOnlyList<UrlEntity>>()))
            .Returns(Fixture.Create<IReadOnlyList<UrlResponse>>());

        var result = await _handler.Handle(mockRequest, It.IsAny<CancellationToken>());

        Assert.NotEmpty(result);
    }

    private GetAllUrlsQuery CreateMockRequest()
    {
        var mockRequest = Fixture
            .Build<GetAllUrlsQuery>()
            .Create();

        return mockRequest;
    }
}