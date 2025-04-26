using Application.Commons.DTO;
using MediatR;

namespace Application.Features.Login;

public sealed class LoginCommand : IRequest<LoginResponse>
{
    public required string Email { get; set; }
    public required string Password { get; set; }
}