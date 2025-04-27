using FluentValidation;
using SocialMediaApi.Data.Repositories;
using SocialMediaApi.Models;
using SocialMediaApi.Models.Dtos;
using SocialMediaApi.Services;

namespace SocialMediaApi.Endpoints;

public static class PostEndpoints
{
    public static void Map(WebApplication app)
    {
        app.MapPost("/posts", async (
            CreatePostDto postDto,
            IPostRepository repo,
            IValidator<CreatePostDto> validator,
            ISanitizerService sanitizer) =>
        {
            var validationResult = await validator.ValidateAsync(postDto);
            if (!validationResult.IsValid)
                return Results.ValidationProblem(validationResult.ToDictionary());

            var post = new Post
            {
                Title = sanitizer.Sanitize(postDto.Title),
                Body = sanitizer.Sanitize(postDto.Body),
                AuthorId = postDto.AuthorId
            };

            var postId = await repo.CreatePostAsync(post);
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
