using Application.Modules.Auth.Login;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace UrlShortenerApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public sealed class AuthController : ControllerBase
{
    private readonly IMediator _mediator;
    
    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }
    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<LoginResponse>> LoginAsync([FromBody] LoginCommand command)
    {
        var response = await _mediator.Send(command);

        return Ok(response);
    }
}