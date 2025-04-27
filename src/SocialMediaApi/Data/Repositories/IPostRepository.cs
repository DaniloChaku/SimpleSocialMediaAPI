using SocialMediaApi.Models;

namespace SocialMediaApi.Data.Repositories;

public interface IPostRepository
{
    Task<int> CreatePostAsync(Post post);
    Task<List<Post>> GetAllPostsAsync();
    Task LikePostAsync(Like like);
}
