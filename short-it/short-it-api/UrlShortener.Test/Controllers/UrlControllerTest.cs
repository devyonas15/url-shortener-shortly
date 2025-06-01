using System.Net;
using Application.Commons.DTO;
using Application.Modules.Url.GenerateUrl;
using Application.Modules.Url.GetAllUrls;
using Application.Modules.Url.GetUrlByBase64Code;
using Application.Modules.Url.GetUrlById;
using Application.Modules.Url.GetUrlsByUserId;
using Application.Modules.Url.RedirectUrl;
using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Moq;
using UrlShortener.Test.Abstractions;
using UrlShortenerApi.Controllers;
using Xunit;

namespace UrlShortener.Test.Controllers;

public sealed class UrlControllerTest : TestFixture<UrlController>
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
    public async Task GivenDataIsFound_WhenGetUrlsByUserIdAsync_ThenReturnsOk()
    {
        var mockUrlResponses = Fixture.CreateMany<UrlResponse>().ToList();
        Mediator.Setup(x => x.Send(It.IsAny<GetUrlsByUserIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(mockUrlResponses);

        var response = await _controller.GetUrlsByUserIdAsync(Fixture.Create<string>());
        var okResponse = response.Result as OkObjectResult;

        Assert.Equal((int)HttpStatusCode.OK, okResponse?.StatusCode);
        Assert.NotNull(okResponse?.Value);
    }

    [Fact]
    public async Task GivenValidBody_WhenGenerateUrlAsync_ThenReturnsCreated()
    {
        var request = Fixture.Create<GenerateUrlCommand>();
        var mockUrlResponse = Fixture.Build<GenerateUrlResponse>()
            .With(x => x.Id, 1)
            .With(x => x.ShortUrl, "https://shorturl.com")
            .Create();
        Mediator.Setup(x => x.Send(It.IsAny<GenerateUrlCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(mockUrlResponse);

        var response = await _controller.GenerateUrlAsync(request);
        var createdResponse = response.Result as CreatedResult;

        Assert.Equal((int)HttpStatusCode.Created, createdResponse?.StatusCode);
        Assert.NotNull(createdResponse?.Value);
    }

    [Fact]
    public async Task GivenValidBody_WhenRedirectUrl_ThenRedirect()
    {
        const string originalUrl = "https://www.google.com";
        const string base64Code = "aWqawi1";
        
        Mediator.Setup(x => x.Send(It.IsAny<RedirectUrlQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(originalUrl);
        
        var response = await _controller.RedirectUrlAsync(base64Code);
        
        var redirectResultResponse = Assert.IsType<RedirectResult>(response);
        Assert.Equal(originalUrl, redirectResultResponse.Url);
    }
}