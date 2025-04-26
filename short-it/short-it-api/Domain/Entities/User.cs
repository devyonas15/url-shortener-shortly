using Domain.Abstractions;

namespace Domain.Entities;

public sealed class User : BaseEntity
{
    public required int UserId { get; set; }
    public required string Password { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
}