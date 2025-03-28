using FluentValidation.Results;

namespace Application.Exceptions;

public sealed class BadRequestException : Exception
{
    public List<string> Errors { get; set; }
    
    public BadRequestException(string message, ValidationResult validationResult) : base(message)
    {
        Errors = validationResult.Errors.Select(x => x.ErrorMessage).ToList();
    }
}