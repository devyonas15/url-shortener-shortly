using Application.Commons.DTO;
using Application.Modules.Url.GenerateUrl;
using Application.Modules.Url.GetAllUrls;
using Application.Modules.Url.GetUrlByBase64Code;
using Application.Modules.Url.GetUrlById;
using Application.Modules.Url.GetUrlsByUserId;
using Application.Modules.Url.RedirectUrl;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace UrlShortenerApi.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public sealed class UrlController : ControllerBase
{
    private readonly IMediator _mediator;

    public UrlController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("all")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IReadOnlyList<UrlResponse>>> GetUrlsAsync()
    {
        var response = await _mediator.Send(new GetAllUrlsQuery());

        return Ok(response);
    }

    [HttpGet("{urlId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<UrlResponse>> GetUrlByIdAsync(int urlId)
    {
        var response = await _mediator.Send(new GetUrlByIdQuery(urlId));

        return Ok(response);
    }

    [HttpGet("user/{userId:}/all")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IReadOnlyList<UrlResponse>>> GetUrlsByUserIdAsync(string userId)
    {
        var response = await _mediator.Send(new GetUrlsByUserIdQuery(userId));

        return Ok(response);
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<UrlResponse>> GetUrlByBase64Async([FromQuery] string base64Value)
    {
        var response = await _mediator.Send(new GetUrlByBase64CodeQuery(base64Value));

        return Ok(response);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<GenerateUrlResponse>> GenerateUrlAsync([FromBody] GenerateUrlCommand command)
    {
        var response = await _mediator.Send(command);

        return Created($"/{response.Id}", response);
    }

    [HttpGet("/{base64Code}")]
    [ProducesResponseType(StatusCodes.Status302Found)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> RedirectUrlAsync(string base64Code)
    {
        var response = await _mediator.Send(new RedirectUrlQuery(base64Code));

        return Redirect(response);
    }
}