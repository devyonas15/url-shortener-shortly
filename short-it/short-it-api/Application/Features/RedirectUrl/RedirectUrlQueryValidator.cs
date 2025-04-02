using Domain.Helpers;
using FluentValidation;

namespace Application.Features.RedirectUrl;

public sealed class RedirectUrlQueryValidator: AbstractValidator<RedirectUrlQuery>
{
    public RedirectUrlQueryValidator()
    {
        RuleFor(x => x.Base64Code)
            .NotEmpty()
            .WithMessage("Base 64 code is required.");
    }
}