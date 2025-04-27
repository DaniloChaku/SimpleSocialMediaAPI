using FluentValidation;
using SocialMediaApi.Models;

namespace SocialMediaApi.Validators;

public class UserValidator : AbstractValidator<User>
{
    public UserValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty()
            .Length(3, 50);
    }
}
