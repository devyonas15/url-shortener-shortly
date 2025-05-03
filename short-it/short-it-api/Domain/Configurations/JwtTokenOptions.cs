using System.Diagnostics.CodeAnalysis;

namespace Domain.Configurations;

[ExcludeFromCodeCoverage]
public sealed class JwtTokenOptions
{
    public required string IssuerKey { get; set; } = string.Empty;
    public required string Issuer { get; set; } = string.Empty;
    public required string Audience { get; set; } = string.Empty;
    public required int ExpiresInMinutes { get; set; } = 60;
}