using SocialMediaApi.Data;
using SocialMediaApi.Endpoints;
using SocialMediaApi.Middleware;

namespace SocialMediaApi.Extensions;

public static class ApplicationBuilderExtensions
{
    public static WebApplication ConfigureMiddleware(this WebApplication app, IWebHostEnvironment environment)
    {
        if (environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Social Media API V1");
            });
        }

        app.UseMiddleware<ErrorHandlingMiddleware>();

        return app;
    }

    public static async Task<WebApplication> InitializeDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var migrationService = scope.ServiceProvider.GetRequiredService<MigrationService>();
        await migrationService.InitializeAsync();

        return app;
    }

    public static WebApplication RegisterEndpoints(this WebApplication app)
    {
        UserEndpoints.Map(app);
        PostEndpoints.Map(app);

        return app;
    }
}
