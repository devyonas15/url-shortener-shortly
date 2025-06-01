using FluentValidation;

namespace Application.Modules.Url.GetUrlsByUserId;

public class GetUrlsByUserIdQueryValidator : AbstractValidator<GetUrlsByUserIdQuery>
{
    public GetUrlsByUserIdQueryValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("UserId cannot be empty");
    }
}