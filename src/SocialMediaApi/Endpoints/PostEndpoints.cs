using FluentValidation;
using SocialMediaApi.Constants;
using SocialMediaApi.Models;
using SocialMediaApi.Models.Dtos;
using SocialMediaApi.Services;

namespace SocialMediaApi.Endpoints;

public static class PostEndpoints
{
    public static void Map(WebApplication app)
    {
        app.MapPost(ApiRoutes.Posts.Create, CreatePost)
            .WithName("CreatePost")
            .WithTags("Posts")
            .Accepts<CreatePostDto>("application/json")
            .Produces<int>(StatusCodes.Status201Created)
            .ProducesValidationProblem();

        app.MapGet(ApiRoutes.Posts.GetAll, GetAllPosts)
            .WithName("GetPosts")
            .WithTags("Posts")
            .Produces<List<PostDto>>(StatusCodes.Status200OK);

        app.MapPost(ApiRoutes.Posts.Like, LikePost)
            .WithName("LikePost")
            .WithTags("Posts")
            .Accepts<Like>("application/json")
            .Produces(StatusCodes.Status200OK)
            .ProducesValidationProblem();
    }

    private static async Task<IResult> CreatePost(
        CreatePostDto postDto,
        IPostService postService,
        IValidator<CreatePostDto> validator)
    {
        var validationResult = await validator.ValidateAsync(postDto);
        if (!validationResult.IsValid)
            return Results.ValidationProblem(validationResult.ToDictionary());

        var postId = await postService.CreatePostAsync(postDto);
        return Results.Created($"/posts/{postId}", postId);
    }

    private static async Task<IResult> GetAllPosts(IPostService postService)
    {
        var posts = await postService.GetAllPostsAsync();
        return Results.Ok(posts);
    }

    private static async Task<IResult> LikePost(
        Like like,
        IPostService postService,
        IValidator<Like> validator)
    {
        var validationResult = await validator.ValidateAsync(like);
        if (!validationResult.IsValid)
            return Results.ValidationProblem(validationResult.ToDictionary());

        await postService.LikePostAsync(like);
        return Results.Ok();
    }
}