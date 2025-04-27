using FluentValidation;
using SocialMediaApi.Models;

namespace SocialMediaApi.Validators;

public class FollowValidator : AbstractValidator<Follow>
{
    public FollowValidator()
    {
        RuleFor(x => x.FollowerId).GreaterThan(0);
        RuleFor(x => x.FollowingId).GreaterThan(0);
        RuleFor(x => x.FollowerId)
            .NotEqual(x => x.FollowingId)
            .WithMessage("Cannot follow yourself.");
    }
}
