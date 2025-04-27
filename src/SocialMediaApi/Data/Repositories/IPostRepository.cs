using SocialMediaApi.Models;

namespace SocialMediaApi.Data.Repositories;

public interface IPostRepository
{
    Task<int> CreatePostAsync(Post post);
    Task<IEnumerable<Post>> GetAllPostsAsync();
    Task LikePostAsync(Like like);
}
