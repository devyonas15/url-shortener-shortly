using Application.Commons.DTO;
using Application.Features.Login;

namespace Application.Contracts.Infrastructure;

public interface IAuthService
{
    // Todo: Change the parameter into user body or anything, not really that neat to use command here
    Task<LoginResponse> SignInAsync (LoginCommand request, CancellationToken cancellationToken = default);
}