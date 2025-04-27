using Moq;
using SocialMediaApi.Data.Repositories;
using SocialMediaApi.Models.Dtos;
using SocialMediaApi.Models;
using SocialMediaApi.Services;

namespace SocialMediaApi.UnitTests.Services;

public class UserServiceTests
{
    private readonly Mock<IUserRepository> _mockRepository;
    private readonly Mock<ISanitizerService> _mockSanitizer;
    private readonly UserService _userService;

    public UserServiceTests()
    {
        _mockRepository = new Mock<IUserRepository>();
        _mockSanitizer = new Mock<ISanitizerService>();
        _userService = new UserService(_mockRepository.Object, _mockSanitizer.Object);
    }

    [Fact]
    public async Task CreateUserAsync_SanitizesUsernameAndSavesToRepository()
    {
        // Arrange
        var userDto = new CreateUserDto
        {
            Username = "<script>alert('xss')</script>TestUser"
        };

        var sanitizedUsername = "TestUser";

        _mockSanitizer.Setup(s => s.Sanitize(userDto.Username)).Returns(sanitizedUsername);
        _mockRepository.Setup(r => r.CreateUserAsync(It.IsAny<User>())).ReturnsAsync(10);

        // Act
        var result = await _userService.CreateUserAsync(userDto);

        // Assert
        Assert.Equal(10, result);
        _mockSanitizer.Verify(s => s.Sanitize(userDto.Username), Times.Once);
        _mockRepository.Verify(r => r.CreateUserAsync(It.Is<User>(u =>
            u.Username == sanitizedUsername)), Times.Once);
    }

    [Fact]
    public async Task FollowUserAsync_CallsRepositoryMethod()
    {
        // Arrange
        var follow = new Follow { FollowerId = 1, FollowingId = 2 };
        _mockRepository.Setup(r => r.FollowUserAsync(It.IsAny<Follow>())).Returns(Task.CompletedTask);

        // Act
        await _userService.FollowUserAsync(follow);

        // Assert
        _mockRepository.Verify(r => r.FollowUserAsync(It.Is<Follow>(f =>
            f.FollowerId == follow.FollowerId &&
            f.FollowingId == follow.FollowingId)), Times.Once);
    }
}
