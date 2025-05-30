using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.Contracts.Infrastructure;
using AutoMapper;
using Domain.Configurations;
using Infrastructure.Abstractions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace Infrastructure.Services;

public sealed class TokenService : BaseService, ITokenService
{
    private readonly JwtTokenOptions _tokenOptions;

    public TokenService(ILogger<TokenService> logger, IMapper mapper, IOptions<JwtTokenOptions> tokenOptions) : base(
        logger,
        mapper)
    {
        _tokenOptions = tokenOptions.Value;
    }

    public string GenerateJwtToken(string userId, string email)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_tokenOptions.IssuerKey);
            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new(JwtRegisteredClaimNames.Sub, userId),
                new(JwtRegisteredClaimNames.Email, email)
            };
            var jwtTokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = _tokenOptions.Issuer,
                Audience = _tokenOptions.Audience,
                Expires = DateTime.UtcNow.AddMinutes(_tokenOptions.ExpiresInMinutes),
                Subject = new ClaimsIdentity(claims),
                SigningCredentials =
                    new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(jwtTokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
        catch (Exception ex)
        {
            throw new Exception($"Unable to generate jwt due to: {ex.Message}");
        }
    }
}