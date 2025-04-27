using FluentValidation;
using SocialMediaApi.Models.Dtos;

namespace SocialMediaApi.Validators;

public class CreatePostDtoValidator : AbstractValidator<CreatePostDto>
{
    public CreatePostDtoValidator()
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
