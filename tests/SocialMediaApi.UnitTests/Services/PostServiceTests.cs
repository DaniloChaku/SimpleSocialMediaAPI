using SocialMediaApi.Data.Repositories;
using SocialMediaApi.Models.Dtos;
using SocialMediaApi.Models;
using SocialMediaApi.Services;
using Moq;

namespace SocialMediaApi.UnitTests.Services;

public class PostServiceTests
{
    private readonly Mock<IPostRepository> _mockRepository;
    private readonly Mock<ISanitizerService> _mockSanitizer;
    private readonly PostService _postService;

    public PostServiceTests()
    {
        _mockRepository = new Mock<IPostRepository>();
        _mockSanitizer = new Mock<ISanitizerService>();
        _postService = new PostService(_mockRepository.Object, _mockSanitizer.Object);
    }

    [Fact]
    public async Task CreatePostAsync_SanitizesInputAndSavesToRepository()
    {
        // Arrange
        var postDto = new CreatePostDto
        {
            Title = "<script>alert('xss')</script>Title",
            Body = "<p>Body with <script>bad script</script></p>",
            AuthorId = 1
        };

        var sanitizedTitle = "Title";
        var sanitizedBody = "<p>Body with </p>";

        _mockSanitizer.Setup(s => s.Sanitize(postDto.Title)).Returns(sanitizedTitle);
        _mockSanitizer.Setup(s => s.Sanitize(postDto.Body)).Returns(sanitizedBody);
        _mockRepository.Setup(r => r.CreatePostAsync(It.IsAny<Post>())).ReturnsAsync(5);

        // Act
        var result = await _postService.CreatePostAsync(postDto);

        // Assert
        Assert.Equal(5, result);
        _mockSanitizer.Verify(s => s.Sanitize(postDto.Title), Times.Once);
        _mockSanitizer.Verify(s => s.Sanitize(postDto.Body), Times.Once);
        _mockRepository.Verify(r => r.CreatePostAsync(It.Is<Post>(p =>
            p.Title == sanitizedTitle &&
            p.Body == sanitizedBody &&
            p.AuthorId == postDto.AuthorId)), Times.Once);
    }

    [Fact]
    public async Task GetAllPostsAsync_ReturnsAllPostsFromRepository()
    {
        // Arrange
        var posts = new List<Post>
            {
                new Post { Id = 1, Title = "Title 1", Body = "Body 1", AuthorId = 1 },
                new Post { Id = 2, Title = "Title 2", Body = "Body 2", AuthorId = 2 }
            };

        _mockRepository.Setup(r => r.GetAllPostsAsync()).ReturnsAsync(posts);

        // Act
        var result = await _postService.GetAllPostsAsync();

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Equal(posts[0].Id, result[0].Id);
        Assert.Equal(posts[0].Title, result[0].Title);
        Assert.Equal(posts[0].Body, result[0].Body);
        Assert.Equal(posts[0].AuthorId, result[0].AuthorId);

        Assert.Equal(posts[1].Id, result[1].Id);
        Assert.Equal(posts[1].Title, result[1].Title);
        Assert.Equal(posts[1].Body, result[1].Body);
        Assert.Equal(posts[1].AuthorId, result[1].AuthorId);

        _mockRepository.Verify(r => r.GetAllPostsAsync(), Times.Once);
    }

    [Fact]
    public async Task LikePostAsync_CallsRepositoryMethod()
    {
        // Arrange
        var like = new Like { PostId = 1, UserId = 2 };
        _mockRepository.Setup(r => r.LikePostAsync(It.IsAny<Like>())).Returns(Task.CompletedTask);

        // Act
        await _postService.LikePostAsync(like);

        // Assert
        _mockRepository.Verify(r => r.LikePostAsync(It.Is<Like>(l =>
            l.PostId == like.PostId &&
            l.UserId == like.UserId)), Times.Once);
    }
}