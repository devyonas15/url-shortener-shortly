using FluentValidation;

namespace Application.Features.GetUrlByBase64Code;

public sealed class GetUrlByBase64CodeValidator: AbstractValidator<GetUrlByBase64CodeQuery>
{
    public GetUrlByBase64CodeValidator()
    {
        RuleFor(q => q.Base64Code)
            .NotEmpty()
            .WithMessage("Base64 Code is required.");
    }
}