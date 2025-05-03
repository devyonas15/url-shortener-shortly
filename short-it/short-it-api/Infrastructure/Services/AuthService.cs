using System.Security.Authentication;
using Application.Commons.DTO;
using Application.Contracts.Infrastructure;
using Application.Exceptions;
using Application.Features.Login;
using AutoMapper;
using Infrastructure.Abstractions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Persistence.Models;

namespace Infrastructure.Services;

public sealed class AuthService : BaseService, IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;

    public AuthService(UserManager<ApplicationUser> userManager, ILogger<AuthService> logger, IMapper mapper) : base(
        logger, mapper)
    {
        _userManager = userManager;
    }

    public async Task<LoginResponse> SignInAsync(LoginCommand request, CancellationToken cancellationToken = default)
    {
        try
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user is null)
            {
                throw new NotFoundException($"User with email {request.Email} does not exist");
            }

            var isPasswordMatches = await _userManager.CheckPasswordAsync(user, request.Password);

            if (!isPasswordMatches)
            {
                throw new InvalidCredentialException($"Credential for user with email {request.Email} are invalid");
            }

            return Mapper.Map<LoginResponse>(user);
        }
        catch (Exception ex)
        {
            if (ex is NotFoundException or BadRequestException)
            {
                throw;
            }

            throw new Exception($"Login for email {request.Email} failed due to: {ex.Message}");
        }
    }
}