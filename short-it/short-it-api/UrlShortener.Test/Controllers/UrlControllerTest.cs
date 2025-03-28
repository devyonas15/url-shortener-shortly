using System.Net;
using Application.Commons.DTO;
using Application.Features.GenerateUrl;
using Application.Features.GetAllUrls;
using Application.Features.GetUrlByBase64Code;
using Application.Features.GetUrlById;
using AutoFixture;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using UrlShortener.Test.Abstractions;
using UrlShortenerApi.Controllers;
using Xunit;

namespace UrlShortener.Test.Controllers;

public sealed class UrlControllerTest : TestFixture
{
    private readonly UrlController _controller = new(Mediator.Object);

    [Fact]
    public async Task GivenDataIsFound_WhenGetUrlByIdAsync_ThenReturnsOk()
    {
        Mediator.Setup(x => x.Send(It.IsAny<GetUrlByIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Fixture.Create<UrlResponse>());
        
        var response = await _controller.GetUrlByIdAsync(Fixture.Create<int>());
        var okResponse = response.Result as OkObjectResult;

        Assert.Equal((int)HttpStatusCode.OK, okResponse?.StatusCode);
        Assert.NotNull(okResponse?.Value);
    }

    [Fact]
    public async Task GivenDataIsFound_WhenGetAllUrls_ThenReturnsOk()
    {
        Mediator.Setup(x => x.Send(It.IsAny<GetAllUrlsQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Fixture.CreateMany<UrlResponse>().ToList());

        var response = await _controller.GetUrlsAsync();
        var okResponse = response.Result as OkObjectResult;
        
        Assert.Equal((int)HttpStatusCode.OK, okResponse?.StatusCode);
    }

    [Fact]
    public async Task GivenDataIsFound_WhenGetUrlByBase64CodeAsync_ThenReturnsOk()
    {
        Mediator.Setup(x => x.Send(It.IsAny<GetUrlByBase64CodeQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Fixture.Create<UrlResponse>());

        var response = await _controller.GetUrlByBase64Async("asdfg3");
        var okResponse = response.Result as OkObjectResult;
        
        Assert.Equal((int)HttpStatusCode.OK, okResponse?.StatusCode);
        Assert.NotNull(okResponse?.Value);
    }

    [Fact]
    public async Task GivenValidBody_WhenGenerateUrlAsync_ThenReturnsCreated()
    {
        var request = Fixture.Create<GenerateUrlCommand>();
        Mediator.Setup(x => x.Send(It.IsAny<GenerateUrlCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);
        
        var response = await _controller.GenerateUrlAsync(request);
        var createdResponse = response.Result as CreatedResult;
        
        Assert.Equal((int)HttpStatusCode.Created, createdResponse?.StatusCode);
        Assert.NotNull(createdResponse?.Value);
    }
}