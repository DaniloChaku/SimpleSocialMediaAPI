using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Testing;
using SocialMediaApi.Middleware;
using System.Text.Json;

namespace SocialMediaApi.UnitTests.Middleware;

public class ErrorHandlingMiddlewareTests
{
    [Fact]
    public async Task InvokeAsync_NoException_CallsNextDelegate()
    {
        // Arrange
        var context = new DefaultHttpContext();
        var fakeLogger = new FakeLogger<ErrorHandlingMiddleware>();

        bool nextDelegateCalled = false;
        RequestDelegate next = (HttpContext ctx) =>
        {
            nextDelegateCalled = true;
            return Task.CompletedTask;
        };

        var middleware = new ErrorHandlingMiddleware(next, fakeLogger);

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        Assert.True(nextDelegateCalled);
        Assert.Empty(fakeLogger.Collector.GetSnapshot());
    }

    [Fact]
    public async Task InvokeAsync_WithException_LogsErrorAndReturns500()
    {
        // Arrange
        var context = new DefaultHttpContext();
        context.Response.Body = new MemoryStream();

        var fakeLogger = new FakeLogger<ErrorHandlingMiddleware>();

        var expectedException = new Exception("Test exception");
        RequestDelegate next = (HttpContext ctx) =>
        {
            throw expectedException;
        };

        var middleware = new ErrorHandlingMiddleware(next, fakeLogger);

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        Assert.Equal(StatusCodes.Status500InternalServerError, context.Response.StatusCode);
        Assert.Contains("application/json", context.Response.ContentType);

        context.Response.Body.Seek(0, SeekOrigin.Begin);
        using var reader = new StreamReader(context.Response.Body);
        var responseBody = await reader.ReadToEndAsync();
        var responseObject = JsonSerializer.Deserialize<ErrorResponse>(responseBody, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        Assert.Equal(StatusCodes.Status500InternalServerError, responseObject!.StatusCode);
        Assert.Equal("An internal server error occurred.", responseObject.Message);

        Assert.Single(fakeLogger.Collector.GetSnapshot());
        var logEntry = fakeLogger.Collector.GetSnapshot()[0];
        Assert.Equal(LogLevel.Error, logEntry.Level);
        Assert.Equal("An unhandled exception occurred", logEntry.Message);
        Assert.Same(expectedException, logEntry.Exception);
    }

    private class ErrorResponse
    {
        public int StatusCode { get; set; } = default;
        public string Message { get; set; } = string.Empty;
    }
}
