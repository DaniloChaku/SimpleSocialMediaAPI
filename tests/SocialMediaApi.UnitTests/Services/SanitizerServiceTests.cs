using SocialMediaApi.Services;

namespace SocialMediaApi.UnitTests.Services;

public class SanitizerServiceTests
{
    [Fact]
    public void Sanitize_RemovesDisallowedTags()
    {
        // Arrange
        var sanitizerService = new SanitizerService();
        var input = "<script>alert('xss')</script><b>Bold</b><p>Paragraph</p>";

        // Act
        var result = sanitizerService.Sanitize(input);

        // Assert
        Assert.DoesNotContain("<script>", result);
        Assert.DoesNotContain("<p>", result);
        Assert.Contains("<b>", result);
    }

    [Fact]
    public void Sanitize_AllowsConfiguredTags()
    {
        // Arrange
        var sanitizerService = new SanitizerService();
        var input = "<b>Bold</b><i>Italic</i><u>Underline</u>";

        // Act
        var result = sanitizerService.Sanitize(input);

        // Assert
        Assert.Equal(input, result);
    }

    [Fact]
    public void Sanitize_WithNullInput_ReturnsEmptyString()
    {
        // Arrange
        var sanitizerService = new SanitizerService();

        // Act
        var result = sanitizerService.Sanitize(null!);

        // Assert
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void Sanitize_WithEmptyInput_ReturnsEmptyString()
    {
        // Arrange
        var sanitizerService = new SanitizerService();

        // Act
        var result = sanitizerService.Sanitize(string.Empty);

        // Assert
        Assert.Equal(string.Empty, result);
    }
}
