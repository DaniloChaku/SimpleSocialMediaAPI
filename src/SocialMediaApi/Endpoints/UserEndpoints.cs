using FluentValidation;
using SocialMediaApi.Data.Repositories;
using SocialMediaApi.Models;
using SocialMediaApi.Models.Dtos;

namespace SocialMediaApi.Endpoints;

public static class UserEndpoints
{
    public static void Map(WebApplication app)
    {
        app.MapPost("/users", async (CreateUserDto user, IUserRepository repo, IValidator<CreateUserDto> validator) =>
        {
            var validationResult = await validator.ValidateAsync(user);
            if (!validationResult.IsValid)
                return Results.ValidationProblem(validationResult.ToDictionary());

            var userId = await repo.CreateUserAsync(user.ToModel());
            return Results.Created($"/users/{userId}", userId);
        })
        .WithName("CreateUser")
        .WithTags("Users")
        .Accepts<CreateUserDto>("application/json")
        .Produces<int>(StatusCodes.Status201Created)
        .ProducesValidationProblem();

        app.MapPost("/users/follow", async (Follow follow, IUserRepository repo, IValidator<Follow> validator) =>
        {
            var validationResult = await validator.ValidateAsync(follow);
            if (!validationResult.IsValid)
                return Results.ValidationProblem(validationResult.ToDictionary());

            await repo.FollowUserAsync(follow);
            return Results.Ok();
        })
        .WithName("FollowUser")
        .WithTags("Users")
        .Accepts<Follow>("application/json")
        .Produces(StatusCodes.Status200OK)
        .ProducesValidationProblem();
    }
}
