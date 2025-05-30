using FluentValidation;

namespace Application.Modules.Auth.Login;

public sealed class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();
        
        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(8)
            .WithMessage("Password must be at least 8 characters long")
            .Matches(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[\W_]).+$")
            .WithMessage("Password must consist of alphanumeric and special characters.");
    }
}