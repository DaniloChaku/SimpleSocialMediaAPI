using FluentValidation;
using SocialMediaApi.Models;

namespace SocialMediaApi.Validators;

public class LikeValidator : AbstractValidator<Like>
{
    public LikeValidator()
    {
        RuleFor(x => x.UserId).GreaterThan(0);
        RuleFor(x => x.PostId).GreaterThan(0);
    }
}
