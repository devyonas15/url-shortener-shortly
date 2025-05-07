using System.Net;
using Application.Commons.DTO;
using Application.Features.Login;
using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Moq;
using UrlShortener.Test.Abstractions;
using UrlShortenerApi.Controllers;
using Xunit;

namespace UrlShortener.Test.Controllers;

public sealed class AuthControllerTest : TestFixture<AuthController>
{
    private readonly AuthController _controller = new(Mediator.Object);

    [Fact]
    public async Task GivenValidRequest_WhenLogin_ThenReturnOkResult()
    {
        var request = Fixture.Build<LoginCommand>()
            .With(x => x.Email, "test@mailinator.com")
            .With(x => x.Password, "secret")
            .Create();
        var expectedResponse = Fixture.Create<LoginResponse>();

        Mediator.Setup(x => x.Send(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        var response = await _controller.LoginAsync(request);

        var okResponse = response.Result as OkObjectResult;
        Assert.Equal((int?)HttpStatusCode.OK, okResponse?.StatusCode);
        Assert.NotNull(okResponse?.Value);
    }
}