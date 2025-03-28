using Application.Contracts.Persistence;
using Application.Exceptions;
using Application.Features.GenerateUrl;
using AutoFixture;
using AutoMapper;
using Domain.Entities;
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
        Fixture.Customize<Url>(c =>
            c.Without(x => x.Metrics));

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
        var request = Fixture.Build<GenerateUrlCommand>()
            .With(x => x.LongUrl, "https://www.test.com")
            .Create();

        UrlRepository.Setup(x => x.GetByLongUrlAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Url?)null);
        UrlRepository.Setup(x => x.CreateAsync(It.IsAny<Url>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var result = await _handler.Handle(request, CancellationToken.None);

        Assert.IsType<int>(result);
        Assert.Equal(0, result);
    }
}