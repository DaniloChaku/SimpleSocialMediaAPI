using FluentValidation;
using SocialMediaApi.Models.Dtos;

namespace SocialMediaApi.Validators;

public class CreateUserDtoValidator : AbstractValidator<CreateUserDto>
{
    public CreateUserDtoValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty()
            .Length(3, 50);
    }
}
