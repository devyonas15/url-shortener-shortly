using MediatR;

namespace Application.Modules.Auth.Login;

public sealed class LoginCommand : IRequest<LoginResponse>
{
    public required string Email { get; set; }
    public required string Password { get; set; }
}