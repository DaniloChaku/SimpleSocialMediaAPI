using SocialMediaApi.Models.Dtos;
using SocialMediaApi.Models;

namespace SocialMediaApi.Services;

public interface IUserService
{
    Task<int> CreateUserAsync(CreateUserDto userDto);
    Task FollowUserAsync(Follow follow);
}
