namespace Domain.Abstractions;

public abstract class BaseEntity
{
    public DateTime DateCreated { get; set; } = DateTime.Now;

    public DateTime DateModified { get; set; } = DateTime.Now;
}