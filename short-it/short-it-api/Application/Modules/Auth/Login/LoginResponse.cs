namespace Application.Modules.Auth.Login;

public sealed class LoginResponse
{
    public required string LoginId { get; set; } = string.Empty;
    public required string Email { get; set; } = string.Empty;
    public required string AccessToken { get; set; } = string.Empty;
}