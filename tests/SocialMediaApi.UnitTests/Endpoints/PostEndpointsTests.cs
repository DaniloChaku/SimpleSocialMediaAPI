using FluentValidation.Results;
using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;
using SocialMediaApi.Constants;
using SocialMediaApi.Endpoints;
using SocialMediaApi.Models.Dtos;
using SocialMediaApi.Models;
using SocialMediaApi.Services;
using Microsoft.AspNetCore.Http;
using System.Reflection;

namespace SocialMediaApi.UnitTests.Endpoints;

public class PostEndpointsTests
{
    private readonly Mock<IPostService> _mockPostService;
    private readonly Mock<IValidator<CreatePostDto>> _mockCreatePostValidator;
    private readonly Mock<IValidator<Like>> _mockLikeValidator;
    private readonly MethodInfo _createPostMethod;
    private readonly MethodInfo _getAllPostsMethod;
    private readonly MethodInfo _likePostMethod;

    public PostEndpointsTests()
    {
        _mockPostService = new Mock<IPostService>();
        _mockCreatePostValidator = new Mock<IValidator<CreatePostDto>>();
        _mockLikeValidator = new Mock<IValidator<Like>>();

        Type postEndpointsType = typeof(PostEndpoints);
        _createPostMethod = postEndpointsType.GetMethod("CreatePost",
            BindingFlags.NonPublic | BindingFlags.Static)!;
        _getAllPostsMethod = postEndpointsType.GetMethod("GetAllPosts",
            BindingFlags.NonPublic | BindingFlags.Static)!;
        _likePostMethod = postEndpointsType.GetMethod("LikePost",
            BindingFlags.NonPublic | BindingFlags.Static)!;
    }

    [Fact]
    public async Task CreatePost_WithValidInput_ReturnsCreatedResult()
    {
        // Arrange
        var postDto = new CreatePostDto
        {
            Title = "Test Title",
            Body = "Test Body",
            AuthorId = 1
        };

        var validationResult = new ValidationResult();
        _mockCreatePostValidator.Setup(v => v.ValidateAsync(postDto, default))
            .ReturnsAsync(validationResult);

        const int expectedPostId = 42;
        _mockPostService.Setup(s => s.CreatePostAsync(postDto))
            .ReturnsAsync(expectedPostId);

        // Act
        var resultTask = (Task<IResult>)_createPostMethod.Invoke(null, new object[]
        {
                postDto,
                _mockPostService.Object,
                _mockCreatePostValidator.Object
        })!;
        var result = await resultTask;

        // Assert
        var createdResult = Assert.IsType<Created<int>>(result);
        Assert.Equal($"{ApiRoutes.Posts.Base}/{expectedPostId}", createdResult.Location);
        Assert.Equal(expectedPostId, createdResult.Value);
        _mockPostService.Verify(s => s.CreatePostAsync(postDto), Times.Once);
    }

    [Fact]
    public async Task CreatePost_WithInvalidInput_ReturnsValidationProblem()
    {
        // Arrange
        var postDto = new CreatePostDto
        {
        };

        var validationFailures = new List<ValidationFailure>
            {
                new ValidationFailure("Title", "Title is required"),
                new ValidationFailure("Body", "Body is required")
            };

        var validationResult = new ValidationResult(validationFailures);
        _mockCreatePostValidator.Setup(v => v.ValidateAsync(postDto, default))
            .ReturnsAsync(validationResult);

        // Act 
        var resultTask = (Task<IResult>)_createPostMethod.Invoke(null,
        [
                postDto,
                _mockPostService.Object,
                _mockCreatePostValidator.Object
        ])!;
        var result = await resultTask;

        // Assert
        var validationProblem = Assert.IsType<ProblemHttpResult>(result);
        var problemDetails = Assert.IsType<HttpValidationProblemDetails>(validationProblem.ProblemDetails);
        var errors = problemDetails.Errors;

        Assert.Equal(2, errors.Count);
        Assert.Contains("Title", errors.Keys);
        Assert.Contains("Body", errors.Keys);

        _mockPostService.Verify(s => s.CreatePostAsync(It.IsAny<CreatePostDto>()), Times.Never);
    }

    [Fact]
    public async Task GetAllPosts_ReturnsOkResultWithPosts()
    {
        // Arrange
        var expectedPosts = new List<PostDto>
            {
                new PostDto { Id = 1, Title = "Post 1", Body = "Body 1", AuthorId = 1 },
                new PostDto { Id = 2, Title = "Post 2", Body = "Body 2", AuthorId = 2 }
            };

        _mockPostService.Setup(s => s.GetAllPostsAsync())
            .ReturnsAsync(expectedPosts);

        // Act
        var resultTask = (Task<IResult>)_getAllPostsMethod.Invoke(null, new object[]
        {
                _mockPostService.Object
        })!;
        var result = await resultTask;

        // Assert
        var okResult = Assert.IsType<Ok<List<PostDto>>>(result);
        Assert.Equal(expectedPosts, okResult.Value);
        _mockPostService.Verify(s => s.GetAllPostsAsync(), Times.Once);
    }

    [Fact]
    public async Task LikePost_WithValidInput_ReturnsOkResult()
    {
        // Arrange
        var like = new Like { PostId = 1, UserId = 2 };

        var validationResult = new ValidationResult();
        _mockLikeValidator.Setup(v => v.ValidateAsync(like, default))
            .ReturnsAsync(validationResult);

        _mockPostService.Setup(s => s.LikePostAsync(like))
            .Returns(Task.CompletedTask);

        // Act
        var resultTask = (Task<IResult>)_likePostMethod.Invoke(null, new object[]
        {
                like,
                _mockPostService.Object,
                _mockLikeValidator.Object
        })!;
        var result = await resultTask;

        // Assert
        Assert.IsType<Ok>(result);
        _mockPostService.Verify(s => s.LikePostAsync(like), Times.Once);
    }

    [Fact]
    public async Task LikePost_WithInvalidInput_ReturnsValidationProblem()
    {
        // Arrange
        var like = new Like { /* Invalid input - missing required fields */ };

        var validationFailures = new List<ValidationFailure>
            {
                new ValidationFailure("PostId", "PostId must be greater than 0"),
                new ValidationFailure("UserId", "UserId must be greater than 0")
            };

        var validationResult = new ValidationResult(validationFailures);
        _mockLikeValidator.Setup(v => v.ValidateAsync(like, default))
            .ReturnsAsync(validationResult);

        // Act
        var resultTask = (Task<IResult>)_likePostMethod.Invoke(null, new object[]
        {
                like,
                _mockPostService.Object,
                _mockLikeValidator.Object
        })!;
        var result = await resultTask;

        // Assert
        var validationProblem = Assert.IsType<ProblemHttpResult>(result);
        var problemDetails = Assert.IsType<HttpValidationProblemDetails>(validationProblem.ProblemDetails);
        var errors = problemDetails.Errors;

        Assert.Equal(2, errors.Count);
        Assert.Contains("PostId", errors.Keys);
        Assert.Contains("UserId", errors.Keys);

        _mockPostService.Verify(s => s.LikePostAsync(It.IsAny<Like>()), Times.Never);
    }
}