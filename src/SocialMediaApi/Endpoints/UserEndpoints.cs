using FluentValidation;
using SocialMediaApi.Data.Repositories;
using SocialMediaApi.Models;

namespace SocialMediaApi.Endpoints;

public static class UserEndpoints
{
    public static void Map(WebApplication app)
    {
        app.MapPost("/users", async (User user, IUserRepository repo, IValidator<User> validator) =>
        {
            var validationResult = await validator.ValidateAsync(user);
            if (!validationResult.IsValid)
                return Results.ValidationProblem(validationResult.ToDictionary());

            var userId = await repo.CreateUserAsync(user);
            return Results.Created($"/users/{userId}", userId);
        });

        app.MapPost("/users/follow", async (Follow follow, IUserRepository repo, IValidator<Follow> validator) =>
        {
            var validationResult = await validator.ValidateAsync(follow);
            if (!validationResult.IsValid)
                return Results.ValidationProblem(validationResult.ToDictionary());

            await repo.FollowUserAsync(follow);
            return Results.Ok();
        });
    }
}
