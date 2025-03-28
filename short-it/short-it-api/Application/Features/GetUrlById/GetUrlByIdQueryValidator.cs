using FluentValidation;

namespace Application.Features.GetUrlById;

public sealed class GetUrlByIdQueryValidator : AbstractValidator<GetUrlByIdQuery>
{
    public GetUrlByIdQueryValidator()
    {
        RuleFor(q => q.UrlId)
            .NotEmpty()
            .WithMessage("Url id is required.");
        
        RuleFor(q => q.UrlId)
            .GreaterThan(0)
            .WithMessage("Url id must be greater than zero.");
    }
}