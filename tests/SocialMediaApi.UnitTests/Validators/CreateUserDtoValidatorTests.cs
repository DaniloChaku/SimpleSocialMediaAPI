using SocialMediaApi.Models.Dtos;
using SocialMediaApi.Models;
using SocialMediaApi.Validators;
using FluentValidation.TestHelper;

namespace SocialMediaApi.UnitTests.Validators;

public class CreateUserDtoValidatorTests
{
    private readonly CreateUserDtoValidator _validator;

    public CreateUserDtoValidatorTests()
    {
        _validator = new CreateUserDtoValidator();
    }

    [Fact]
    public void Should_Pass_When_Valid()
    {
        var dto = new CreateUserDto
        {
            Username = "ValidUsername"
        };

        var result = _validator.TestValidate(dto);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("ab")]
    [InlineData("abcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyz")]
    public void Should_Fail_When_Username_Is_Invalid(string username)
    {
        var dto = new CreateUserDto
        {
            Username = username
        };

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.Username);
    }
}