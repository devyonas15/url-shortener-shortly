using Application.Abstractions;
using Application.Commons.DTO;
using Application.Contracts.Infrastructure;
using Application.Exceptions;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.Login;

public sealed class LoginCommandHandler : BaseHandlerWithValidator<LoginCommand, LoginResponse, LoginCommandValidator>, IRequestHandler<LoginCommand, LoginResponse>
{
    private readonly IAuthService _authService;
    
    public LoginCommandHandler(IMapper mapper, ILogger<LoginCommandHandler> logger, IAuthService authService) : base(mapper, logger)
    {
        _authService = authService;
    }

    public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken = default)
    {
        try
        {
            var validationResult = await Validator.ValidateAsync(request, cancellationToken);
            
            if (!validationResult.IsValid)
            {
                throw new BadRequestException("Validation payload:", validationResult);
            }

            var response = await _authService.SignInAsync(request, cancellationToken);

            return response;
        }
        catch (Exception ex)
        {
            if (ex is NotFoundException or BadRequestException)
            {
                throw;
            }
            
            throw new Exception($"Failed to login for user {request.Email}: {ex.Message}");
        }

    }
}