using Application.Commons.DTO;
using Application.Contracts.Persistence;
using Application.Exceptions;
using Application.Modules.Url.GetUrlsByUserId;
using AutoFixture;
using Moq;
using UrlShortener.Test.Abstractions;
using Xunit;
using UrlEntity = Domain.Entities.Url;

namespace UrlShortener.Test.Applications.Modules.Url.GetUrlsByUserId;

public sealed class GetUrlsByUserIdQueryHandlerTest : TestFixture<GetUrlsByUserIdQueryHandler>
{
    private static readonly Mock<IUrlRepository> UrlRepository = new();
    private readonly GetUrlsByUserIdQueryHandler _handler = new(Mapper.Object, UrlRepository.Object, Logger.Object);

    [Fact]
    public async Task GivenInvalidId_ThenBadRequestException()
    {
        var mockRequest = Fixture.Build<GetUrlsByUserIdQuery>()
            .With(x => x.UserId, "")
            .Create();

        await Assert.ThrowsAsync<BadRequestException>(async () =>
            await _handler.Handle(mockRequest, It.IsAny<CancellationToken>()));
    }

    [Fact]
    public async Task GivenValidId_WhenRecordNotFound_ThenThrowNotFoundException()
    {
        var mockRequest = Fixture.Build<GetUrlsByUserIdQuery>()
            .With(x => x.UserId, "AAAA1111-AAA1-1111-AA1A-111111AA1111")
            .Create();
        
        UrlRepository.Setup(x => x.GetUrlsByUserIdAsync(mockRequest.UserId, CancellationToken.None))
            .ReturnsAsync((IReadOnlyList<UrlEntity>?)null);

        await Assert.ThrowsAsync<NotFoundException>(async () =>
            await _handler.Handle(mockRequest, It.IsAny<CancellationToken>()));
    }

    [Fact]
    public async Task GivenValidId_WhenRecordIsFound_ThenReturnResponse()
    {
        var mockRequest = Fixture.Build<GetUrlsByUserIdQuery>()
            .With(x => x.UserId, "AAAA1111-AAA1-1111-AA1A-111111AA1111")
            .Create();
        var mockUrls = Fixture.CreateMany<UrlEntity>().ToList();
        var mockUrlResponse = Fixture.CreateMany<UrlResponse>().ToList();
    
        UrlRepository.Setup(x => x.GetUrlsByUserIdAsync(mockRequest.UserId, CancellationToken.None))
            .ReturnsAsync(mockUrls);
        Mapper.Setup(x => x.Map<IReadOnlyList<UrlResponse>>(mockUrls))
            .Returns(mockUrlResponse);
    
        var response = await _handler.Handle(mockRequest, It.IsAny<CancellationToken>());
    
        Assert.NotNull(response);
    }
}