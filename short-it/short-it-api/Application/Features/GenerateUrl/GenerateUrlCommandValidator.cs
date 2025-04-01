using Domain.Helpers;
using FluentValidation;

namespace Application.Features.GenerateUrl;

public sealed class GenerateUrlCommandValidator : AbstractValidator<GenerateUrlCommand>
{
    public GenerateUrlCommandValidator()
    {
        RuleFor(x => x.LongUrl)
            .NotEmpty()
            .WithMessage("Url is required.")
            .Must(UrlHelpers.IsUrlValid)
            .WithMessage("Invalid URL format.");
    }
}