using System.Security.Authentication;
using Application.Abstractions;
using Application.Commons.DTO;
using Application.Contracts.Infrastructure;
using Application.Exceptions;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.Login;

public sealed class LoginCommandHandler : BaseHandlerWithValidator<LoginCommand, LoginResponse, LoginCommandValidator>,
    IRequestHandler<LoginCommand, LoginResponse>
{
    private readonly IAuthService _authService;
    private readonly ITokenService _tokenService;

    public LoginCommandHandler(IMapper mapper, ILogger<LoginCommandHandler> logger, IAuthService authService,
        ITokenService tokenService) : base(mapper, logger)
    {
        _authService = authService;
        _tokenService = tokenService;
    }

    public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken = default)
    {
        try
        {
            Logger.LogInformation($"Attempting to login for user: {request.Email}");
            var validationResult = await Validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                throw new BadRequestException("Validation payload:", validationResult);
            }

            var response = await _authService.SignInAsync(request, cancellationToken);

            // Append JWT token after successful sign in
            var accessToken = _tokenService.GenerateJwtToken(response.LoginId, response.Email);

            response.AccessToken = accessToken;

            Logger.LogInformation($"Successfully login for user: {request.Email}");
            return response;
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, $"Failed to login for user: {request.Email}");
            if (ex is NotFoundException or BadRequestException or InvalidCredentialException)
            {
                throw;
            }

            throw new Exception($"Failed to login for user {request.Email}: {ex.Message}");
        }
    }
}