using SocialMediaApi.Models.Dtos;
using SocialMediaApi.Models;

namespace SocialMediaApi.Services;

public interface IPostService
{
    Task<int> CreatePostAsync(CreatePostDto postDto);
    Task<List<PostDto>> GetAllPostsAsync();
    Task LikePostAsync(Like like);
}
