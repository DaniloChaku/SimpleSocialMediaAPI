using SocialMediaApi.Data.Repositories;
using SocialMediaApi.Models;

namespace SocialMediaApi.Endpoints;

public static class UserEndpoints
{
    public static void Map(WebApplication app)
    {
        app.MapPost("/users", async (User user, IUserRepository repo) =>
        {
            var userId = await repo.CreateUserAsync(user);
            return Results.Created($"/users/{userId}", userId);
        });

        app.MapPost("/users/follow", async (Follow follow, IUserRepository repo) =>
        {
            await repo.FollowUserAsync(follow);
            return Results.Ok();
        });
    }
}
