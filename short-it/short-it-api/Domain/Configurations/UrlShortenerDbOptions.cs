using System.Diagnostics.CodeAnalysis;

namespace Domain.Configurations;

[ExcludeFromCodeCoverage]
public sealed class UrlShortenerDbOptions
{
    public required string Host { get; set; }

    public required string Database { get; set; }

    public required string Username { get; set; }

    public required string Password { get; set; }

    public string ConnectionString => $"Server={Host};Database={Database};User Id={Username};Password={Password};Encrypt=False;TrustServerCertificate=True;";
}