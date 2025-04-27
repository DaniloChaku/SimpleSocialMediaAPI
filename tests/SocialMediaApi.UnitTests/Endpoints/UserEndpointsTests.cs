using System.Reflection;
using FluentValidation.Results;
using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Http;
using Moq;
using SocialMediaApi.Endpoints;
using SocialMediaApi.Models.Dtos;
using SocialMediaApi.Models;
using SocialMediaApi.Services;

namespace SocialMediaApi.UnitTests.Endpoints;

public class UserEndpointsTests
{
    private readonly Mock<IUserService> _mockUserService;
    private readonly Mock<IValidator<CreateUserDto>> _mockCreateUserValidator;
    private readonly Mock<IValidator<Follow>> _mockFollowValidator;
    private readonly MethodInfo _createUserMethod;
    private readonly MethodInfo _followUserMethod;

    public UserEndpointsTests()
    {
        _mockUserService = new Mock<IUserService>();
        _mockCreateUserValidator = new Mock<IValidator<CreateUserDto>>();
        _mockFollowValidator = new Mock<IValidator<Follow>>();

        Type userEndpointsType = typeof(UserEndpoints);
        _createUserMethod = userEndpointsType.GetMethod("CreateUser", BindingFlags.NonPublic | BindingFlags.Static)!;
        _followUserMethod = userEndpointsType.GetMethod("FollowUser", BindingFlags.NonPublic | BindingFlags.Static)!;
    }

    [Fact]
    public async Task CreateUser_WithValidInput_ReturnsCreatedResult()
    {
        // Arrange
        var userDto = new CreateUserDto
        {
            Username = "testuser"
        };

        var validationResult = new ValidationResult();
        _mockCreateUserValidator.Setup(v => v.ValidateAsync(userDto, default))
            .ReturnsAsync(validationResult);

        const int expectedUserId = 101;
        _mockUserService.Setup(s => s.CreateUserAsync(userDto))
            .ReturnsAsync(expectedUserId);

        // Act
        var resultTask = (Task<IResult>)_createUserMethod.Invoke(null, new object[]
        {
            userDto,
            _mockUserService.Object,
            _mockCreateUserValidator.Object
        })!;
        var result = await resultTask;

        // Assert
        var createdResult = Assert.IsType<Created<int>>(result);
        Assert.Equal($"/users/{expectedUserId}", createdResult.Location);
        Assert.Equal(expectedUserId, createdResult.Value);

        _mockUserService.Verify(s => s.CreateUserAsync(userDto), Times.Once);
    }

    [Fact]
    public async Task CreateUser_WithInvalidInput_ReturnsValidationProblem()
    {
        // Arrange
        var userDto = new CreateUserDto();

        var validationFailures = new List<ValidationFailure>
        {
            new ValidationFailure("Username", "Username is required")
        };

        var validationResult = new ValidationResult(validationFailures);
        _mockCreateUserValidator.Setup(v => v.ValidateAsync(userDto, default))
            .ReturnsAsync(validationResult);

        // Act
        var resultTask = (Task<IResult>)_createUserMethod.Invoke(null, new object[]
        {
            userDto,
            _mockUserService.Object,
            _mockCreateUserValidator.Object
        })!;
        var result = await resultTask;

        // Assert
        var validationProblem = Assert.IsType<ProblemHttpResult>(result);
        var problemDetails = Assert.IsType<HttpValidationProblemDetails>(validationProblem.ProblemDetails);

        var errors = problemDetails.Errors;

        Assert.Single(errors);
        Assert.Contains("Username", errors.Keys);

        _mockUserService.Verify(s => s.CreateUserAsync(It.IsAny<CreateUserDto>()), Times.Never);
    }

    [Fact]
    public async Task FollowUser_WithValidInput_ReturnsOkResult()
    {
        // Arrange
        var follow = new Follow
        {
            FollowerId = 1,
            FollowingId = 2
        };

        var validationResult = new ValidationResult();
        _mockFollowValidator.Setup(v => v.ValidateAsync(follow, default))
            .ReturnsAsync(validationResult);

        // Act
        var resultTask = (Task<IResult>)_followUserMethod.Invoke(null, new object[]
        {
            follow,
            _mockUserService.Object,
            _mockFollowValidator.Object
        })!;
        var result = await resultTask;

        // Assert
        Assert.IsType<Ok>(result);

        _mockUserService.Verify(s => s.FollowUserAsync(follow), Times.Once);
    }

    [Fact]
    public async Task FollowUser_WithInvalidInput_ReturnsValidationProblem()
    {
        // Arrange
        var follow = new Follow();

        var validationFailures = new List<ValidationFailure>
        {
            new ValidationFailure("FollowerId", "FollowerId must be greater than 0"),
            new ValidationFailure("FollowingId", "FollowingId must be greater than 0")
        };

        var validationResult = new ValidationResult(validationFailures);
        _mockFollowValidator.Setup(v => v.ValidateAsync(follow, default))
            .ReturnsAsync(validationResult);

        // Act
        var resultTask = (Task<IResult>)_followUserMethod.Invoke(null, new object[]
        {
            follow,
            _mockUserService.Object,
            _mockFollowValidator.Object
        })!;
        var result = await resultTask;

        // Assert
        var validationProblem = Assert.IsType<ProblemHttpResult>(result);
        var problemDetails = Assert.IsType<HttpValidationProblemDetails>(validationProblem.ProblemDetails);

        var errors = problemDetails.Errors;

        Assert.Equal(2, errors.Count);
        Assert.Contains("FollowerId", errors.Keys);
        Assert.Contains("FollowingId", errors.Keys);

        _mockUserService.Verify(s => s.FollowUserAsync(It.IsAny<Follow>()), Times.Never);
    }
}
