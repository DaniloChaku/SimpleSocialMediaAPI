using FluentValidation;
using SocialMediaApi.Data.Repositories;
using SocialMediaApi.Models;
using SocialMediaApi.Models.Dtos;

namespace SocialMediaApi.Endpoints;

public static class PostEndpoints
{
    public static void Map(WebApplication app)
    {
        app.MapPost("/posts", async (CreatePostDto post, IPostRepository repo, IValidator<CreatePostDto> validator) =>
        {
            var validationResult = await validator.ValidateAsync(post);
            if (!validationResult.IsValid)
                return Results.ValidationProblem(validationResult.ToDictionary());

            var postId = await repo.CreatePostAsync(post.ToModel());
            return Results.Created($"/posts/{postId}", postId);
        })
        .WithName("CreatePost")
        .WithTags("Posts")
        .Accepts<CreatePostDto>("application/json")
        .Produces<int>(StatusCodes.Status201Created)
        .ProducesValidationProblem();

        app.MapGet("/posts", async (IPostRepository repo) =>
        {
            var posts = await repo.GetAllPostsAsync();
            return Results.Ok(posts);
        })
        .WithName("GetPosts")
        .WithTags("Posts")
        .Produces<List<Post>>(StatusCodes.Status200OK);

        app.MapPost("/posts/like", async (Like like, IPostRepository repo, IValidator<Like> validator) =>
        {
            var validationResult = await validator.ValidateAsync(like);
            if (!validationResult.IsValid)
                return Results.ValidationProblem(validationResult.ToDictionary());

            await repo.LikePostAsync(like);
            return Results.Ok();
        })
        .WithName("LikePost")
        .WithTags("Posts")
        .Accepts<Like>("application/json")
        .Produces<int>(StatusCodes.Status200OK)
        .ProducesValidationProblem();
    }
}
