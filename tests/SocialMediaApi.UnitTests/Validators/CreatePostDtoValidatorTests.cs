using FluentValidation.TestHelper;
using SocialMediaApi.Models.Dtos;
using SocialMediaApi.Validators;

namespace SocialMediaApi.UnitTests.Validators;

public class CreatePostDtoValidatorTests
{
    private readonly CreatePostDtoValidator _validator;

    public CreatePostDtoValidatorTests()
    {
        _validator = new CreatePostDtoValidator();
    }

    [Fact]
    public void Should_Pass_When_Valid()
    {
        var dto = new CreatePostDto
        {
            Title = "Valid Title",
            Body = "This is a valid body with enough length",
            AuthorId = 1
        };

        var result = _validator.TestValidate(dto);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("123")]
    [InlineData("Very long title that exceeds the 100 characters limit and should not be accepted in any case whatsoever.")]
    public void Should_Fail_When_Title_Is_Invalid(string title)
    {
        var dto = new CreatePostDto
        {
            Title = title,
            Body = "This is a valid body with enough length",
            AuthorId = 1
        };

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.Title);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("Too short")]
    public void Should_Fail_When_Body_Is_Invalid(string body)
    {
        var dto = new CreatePostDto
        {
            Title = "Valid Title",
            Body = body,
            AuthorId = 1
        };

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.Body);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Should_Fail_When_AuthorId_Is_Not_Positive(int authorId)
    {
        var dto = new CreatePostDto
        {
            Title = "Valid Title",
            Body = "This is a valid body with enough length",
            AuthorId = authorId
        };

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.AuthorId);
    }
}


