using Application.Commons.DTO;
using Application.Contracts.Persistence;
using Application.Exceptions;
using Application.Modules.Url.GetUrlById;
using AutoFixture;
using Moq;
using UrlShortener.Test.Abstractions;
using Xunit;
using UrlEntity = Domain.Entities.Url;

namespace UrlShortener.Test.Applications.Modules.Url.GetUrlById;

public sealed class GetUrlByIdHandlerTest : TestFixture<GetUrlByIdQueryHandler>
{
    private static readonly Mock<IUrlRepository> UrlRepository = new();
    private readonly GetUrlByIdQueryHandler _handler = new(Mapper.Object, UrlRepository.Object, Logger.Object);

    [Fact]
    public async Task GivenInvalidId_ThenBadRequestException()
    {
        var mockRequest = Fixture.Build<GetUrlByIdQuery>()
            .With(x => x.UrlId, -1)
            .Create();

        await Assert.ThrowsAsync<BadRequestException>(async () =>
            await _handler.Handle(mockRequest, It.IsAny<CancellationToken>()));
    }

    [Fact]
    public async Task GivenValidId_WhenRecordNotFound_ThenThrowNotFoundException()
    {
        var mockRequest = Fixture.Build<GetUrlByIdQuery>()
            .With(x => x.UrlId, 1)
            .Create();
        UrlRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>(), CancellationToken.None))
            .ReturnsAsync((UrlEntity?)null);

        await Assert.ThrowsAsync<NotFoundException>(async () =>
            await _handler.Handle(mockRequest, It.IsAny<CancellationToken>()));
    }

    [Fact]
    public async Task GivenValidId_WhenRecordIsFound_ThenReturnResponse()
    {
        var mockRequest = Fixture.Build<GetUrlByIdQuery>()
            .With(x => x.UrlId, 1)
            .Create();
        var mockUrl = Fixture.Create<UrlEntity>();
        var mockUrlResponse = Fixture.Create<UrlResponse>();

        UrlRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>(), CancellationToken.None))
            .ReturnsAsync(mockUrl);
        Mapper.Setup(x => x.Map<UrlResponse>(It.IsAny<UrlEntity>()))
            .Returns(mockUrlResponse);

        var response = await _handler.Handle(mockRequest, It.IsAny<CancellationToken>());

        Assert.NotNull(response);
    }
}