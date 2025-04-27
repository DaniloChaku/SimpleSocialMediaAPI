using FluentValidation.TestHelper;
using SocialMediaApi.Models;
using SocialMediaApi.Validators;

namespace SocialMediaApi.UnitTests.Validators;
public class FollowValidatorTests
{
    private readonly FollowValidator _validator;

    public FollowValidatorTests()
    {
        _validator = new FollowValidator();
    }

    [Fact]
    public void Should_Pass_When_Valid()
    {
        var follow = new Follow
        {
            FollowerId = 1,
            FollowingId = 2
        };

        var result = _validator.TestValidate(follow);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Theory]
    [InlineData(0, 1)]
    [InlineData(-1, 1)]
    public void Should_Fail_When_FollowerId_Is_Not_Positive(int followerId, int followingId)
    {
        var follow = new Follow
        {
            FollowerId = followerId,
            FollowingId = followingId
        };

        var result = _validator.TestValidate(follow);

        result.ShouldHaveValidationErrorFor(x => x.FollowerId);
    }

    [Theory]
    [InlineData(1, 0)]
    [InlineData(1, -1)]
    public void Should_Fail_When_FollowingId_Is_Not_Positive(int followerId, int followingId)
    {
        var follow = new Follow
        {
            FollowerId = followerId,
            FollowingId = followingId
        };

        var result = _validator.TestValidate(follow);

        result.ShouldHaveValidationErrorFor(x => x.FollowingId);
    }

    [Fact]
    public void Should_Fail_When_FollowerId_Equals_FollowingId()
    {
        var follow = new Follow
        {
            FollowerId = 1,
            FollowingId = 1
        };

        var result = _validator.TestValidate(follow);

        result.ShouldHaveValidationErrorFor(x => x.FollowerId);
    }
}