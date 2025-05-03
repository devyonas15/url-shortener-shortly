namespace Application.Contracts.Infrastructure;

public interface ITokenService
{
    string GenerateJwtToken(string userId, string email);
}