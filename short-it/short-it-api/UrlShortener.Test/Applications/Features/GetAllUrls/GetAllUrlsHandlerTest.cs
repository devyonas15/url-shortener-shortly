using Application.Commons.DTO;
using Application.Contracts.Persistence;
using Application.Exceptions;
using Application.Features.GetAllUrls;
using AutoFixture;
using AutoMapper;
using Domain.Entities;
using Moq;
using UrlShortener.Test.Abstractions;
using Xunit;

namespace UrlShortener.Test.Applications.Features.GetAllUrls;

public sealed class GetAllUrlsHandlerTest : TestFixture
{
    private static readonly Mock<IUrlRepository> UrlRepository = new();
    private readonly GetAllUrlsQueryHandler _handler = new(UrlRepository.Object, Mapper.Object);

    [Fact]
    public async Task GivenDataNotFound_ThenThrowNotFoundException()
    {
        var mockRequest = CreateMockRequest();
        
        UrlRepository.Setup(x => x.GetAllAsync(CancellationToken.None))
            .ReturnsAsync(new List<Url>());
        
        await Assert.ThrowsAsync<NotFoundException>(async () => await _handler.Handle(mockRequest, It.IsAny<CancellationToken>()));;
    }

    [Fact]
    public async Task GivenDataIsFound_ThenReturnUrls()
    {
        Fixture.Customize<Url>(c => c
            .Without(u => u.Metrics));
            
        var mockRequest = CreateMockRequest();
        
        UrlRepository.Setup(x => x.GetAllAsync(CancellationToken.None))
            .ReturnsAsync(Fixture
                .CreateMany<Url>().ToList());
        
        Mapper.Setup(x => x.Map<IReadOnlyList<UrlResponse>>(It.IsAny<IReadOnlyList<Url>>()))
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