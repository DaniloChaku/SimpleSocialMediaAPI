using FluentValidation.TestHelper;
using SocialMediaApi.Models;
using SocialMediaApi.Validators;

namespace SocialMediaApi.UnitTests.Validators;

public class LikeValidatorTests
{
    private readonly LikeValidator _validator;

    public LikeValidatorTests()
    {
        _validator = new LikeValidator();
    }

    [Fact]
    public void Should_Pass_When_Valid()
    {
        var like = new Like
        {
            UserId = 1,
            PostId = 2
        };

        var result = _validator.TestValidate(like);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Theory]
    [InlineData(0, 1)]
    [InlineData(-1, 1)]
    public void Should_Fail_When_UserId_Is_Not_Positive(int userId, int postId)
    {
        var like = new Like
        {
            UserId = userId,
            PostId = postId
        };

        var result = _validator.TestValidate(like);

        result.ShouldHaveValidationErrorFor(x => x.UserId);
    }

    [Theory]
    [InlineData(1, 0)]
    [InlineData(1, -1)]
    public void Should_Fail_When_PostId_Is_Not_Positive(int userId, int postId)
    {
        var like = new Like
        {
            UserId = userId,
            PostId = postId
        };

        var result = _validator.TestValidate(like);

        result.ShouldHaveValidationErrorFor(x => x.PostId);
    }
}