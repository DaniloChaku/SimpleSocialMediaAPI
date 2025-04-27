using FluentValidation;
using Microsoft.Extensions.Hosting;
using SocialMediaApi.Data.Repositories;
using SocialMediaApi.Models;

namespace SocialMediaApi.Endpoints;

public static class PostEndpoints
{
    public static void Map(WebApplication app)
    {
        app.MapPost("/posts", async (Post post, IPostRepository repo, IValidator<Post> validator) =>
        {
            var validationResult = await validator.ValidateAsync(post);
            if (!validationResult.IsValid)
                return Results.ValidationProblem(validationResult.ToDictionary());

            var postId = await repo.CreatePostAsync(post);
            return Results.Created($"/posts/{postId}", postId);
        });

        app.MapGet("/posts", async (IPostRepository repo) =>
        {
            var posts = await repo.GetAllPostsAsync();
            return Results.Ok(posts);
        });

        app.MapPost("/posts/like", async (Like like, IPostRepository repo, IValidator<Like> validator) =>
        {
            var validationResult = await validator.ValidateAsync(like);
            if (!validationResult.IsValid)
                return Results.ValidationProblem(validationResult.ToDictionary());

            await repo.LikePostAsync(like);
            return Results.Ok();
        });
    }
}
