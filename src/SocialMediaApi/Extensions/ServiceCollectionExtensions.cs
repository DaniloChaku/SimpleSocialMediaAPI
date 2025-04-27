using SocialMediaApi.Data.Repositories;
using SocialMediaApi.Data;
using SocialMediaApi.Services;

namespace SocialMediaApi.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddSingleton<DapperContext>();
        services.AddScoped<MigrationService>();
        services.AddSingleton<ISanitizerService, SanitizerService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IPostService, PostService>();
        services.AddEndpointsApiExplorer();

        return services;
    }

    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IPostRepository, PostRepository>();

        return services;
    }

    public static IServiceCollection AddApiDocumentation(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
            {
                Title = "Social Media API",
                Version = "v1",
                Description = "A simple Minimal API for a social media app.",
            });
        });

        return services;
    }
}
