using FluentValidation;
using SocialMediaApi.Models;
using SocialMediaApi.Models.Dtos;
using SocialMediaApi.Services;

namespace SocialMediaApi.Endpoints;

public static class UserEndpoints
{
    public static void Map(WebApplication app)
    {
        app.MapPost("/users", CreateUser)
            .WithName("CreateUser")
            .WithTags("Users")
            .Accepts<CreateUserDto>("application/json")
            .Produces<int>(StatusCodes.Status201Created)
            .ProducesValidationProblem();

        app.MapPost("/users/follow", FollowUser)
            .WithName("FollowUser")
            .WithTags("Users")
            .Accepts<Follow>("application/json")
            .Produces(StatusCodes.Status200OK)
            .ProducesValidationProblem();
    }

    private static async Task<IResult> CreateUser(
        CreateUserDto userDto,
        IUserService userService,
        IValidator<CreateUserDto> validator)
    {
        var validationResult = await validator.ValidateAsync(userDto);
        if (!validationResult.IsValid)
            return Results.ValidationProblem(validationResult.ToDictionary());

        var userId = await userService.CreateUserAsync(userDto);
        return Results.Created($"/users/{userId}", userId);

    }

    private static async Task<IResult> FollowUser(
        Follow follow,
        IUserService userService,
        IValidator<Follow> validator)
    {
        var validationResult = await validator.ValidateAsync(follow);
        if (!validationResult.IsValid)
            return Results.ValidationProblem(validationResult.ToDictionary());

        await userService.FollowUserAsync(follow);
        return Results.Ok();
    }
}