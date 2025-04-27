using FluentValidation;
using SocialMediaApi.Models;

namespace SocialMediaApi.Validators;

public class PostValidator : AbstractValidator<Post>
{
    public PostValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .Length(5, 100);

        RuleFor(x => x.Body)
            .NotEmpty()
            .MinimumLength(10);

        RuleFor(x => x.AuthorId)
            .GreaterThan(0);
    }
}
