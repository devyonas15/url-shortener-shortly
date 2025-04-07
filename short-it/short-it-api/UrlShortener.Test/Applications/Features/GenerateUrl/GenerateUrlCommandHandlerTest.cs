using Application.Commons.DTO;
using Application.Contracts.Persistence;
using Application.Exceptions;
using Application.Features.GenerateUrl;
using AutoFixture;
using AutoMapper;
using Domain.Entities;
using Domain.Extensions;
using Moq;
using UrlShortener.Test.Abstractions;
using Xunit;

namespace UrlShortener.Test.Applications.Features.GenerateUrl;

public sealed class GenerateUrlCommandHandlerTest : TestFixture<GenerateUrlCommandHandler>
{
    private static readonly Mock<IUrlRepository> UrlRepository = new();
    private readonly GenerateUrlCommandHandler _handler = new(UrlRepository.Object, Mapper.Object, Logger.Object);

    [Fact]
    public async Task GivenInvalidLongUrl_WhenGenerateUrlCommand_ThenThrowsBadRequestException()
    {
        var request = Fixture.Build<GenerateUrlCommand>()
            .With(x => x.LongUrl, "")
            .Create();

        await Assert.ThrowsAsync<BadRequestException>(
            async () => await _handler.Handle(request, CancellationToken.None));
    }

    [Fact]
    public async Task GivenValidLongUrl_WhenGenerateUrlCommand_AndDuplicateUrlFound_ThenThrowsDuplicateException()
    {
        var request = Fixture.Build<GenerateUrlCommand>()
            .With(x => x.LongUrl, "https://www.example.com")
            .Create();
        var duplicateUrl = Fixture.Create<Url>();

        UrlRepository.Setup(x => x.GetByLongUrlAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(duplicateUrl);

        await Assert.ThrowsAsync<DuplicateException>(async () =>
            await _handler.Handle(request, CancellationToken.None));
    }

    [Fact]
    public async Task GivenValidLongUrl_WhenGenerateUrlCommand_AndDuplicateUrlNotFound_ThenSucceeds()
    {
        const int urlId = 1;
        const string longUrl = "https://www.example.com";
        const bool isSuccess = true;
        var request = Fixture.Build<GenerateUrlCommand>()
            .With(x => x.LongUrl, longUrl)
            .Create();
        var response = Fixture.Build<GenerateUrlResponse>()
            .With(x => x.Id, urlId)
            .With(x => x.ShortUrl, longUrl.ToBase64Prefix())
            .With(x => x.Success, isSuccess)
            .Create();

        UrlRepository.Setup(x => x.GetByLongUrlAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Url?)null);
        UrlRepository.Setup(x => x.CreateAsync(It.IsAny<Url>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        Mapper.Setup(x => x.Map<GenerateUrlResponse>(It.IsAny<Url>()))
            .Returns(response);
        var result = await _handler.Handle(request, CancellationToken.None);

        Assert.IsType<GenerateUrlResponse>(result);
        Assert.Equal(urlId, result.Id);
        Assert.Equal(longUrl.ToBase64Prefix(), result.ShortUrl);
        Assert.True(result.Success);
    }
}