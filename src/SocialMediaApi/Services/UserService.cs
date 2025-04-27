using SocialMediaApi.Data.Repositories;
using SocialMediaApi.Models.Dtos;
using SocialMediaApi.Models;

namespace SocialMediaApi.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _repository;
    private readonly ISanitizerService _sanitizer;

    public UserService(IUserRepository repository, ISanitizerService sanitizer)
    {
        _repository = repository;
        _sanitizer = sanitizer;
    }

    public async Task<int> CreateUserAsync(CreateUserDto userDto)
    {
        var user = new User
        {
            Username = _sanitizer.Sanitize(userDto.Username),
        };

        return await _repository.CreateUserAsync(user);
    }

    public async Task FollowUserAsync(Follow follow)
    {
        await _repository.FollowUserAsync(follow);
    }
}
