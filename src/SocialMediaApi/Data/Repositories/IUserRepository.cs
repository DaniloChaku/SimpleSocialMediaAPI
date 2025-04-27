using SocialMediaApi.Models;

namespace SocialMediaApi.Data.Repositories;

public interface IUserRepository
{
    Task<int> CreateUserAsync(User user);
    Task FollowUserAsync(Follow follow);
}
