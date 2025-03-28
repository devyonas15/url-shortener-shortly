using FluentValidation;

namespace Application.Features.GenerateUrl;

public sealed class GenerateUrlCommandValidator : AbstractValidator<GenerateUrlCommand>
{
    public GenerateUrlCommandValidator()
    {
        RuleFor(x => x.LongUrl)
            .NotEmpty()
            .WithMessage("Url is required.")
            .Must(IsUrlValid)
            .WithMessage("Invalid URL format.");
    }
    
    private static bool IsUrlValid(string url)
    {
        return Uri.TryCreate(url, UriKind.Absolute, out var uriResult)
               && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
    }
}