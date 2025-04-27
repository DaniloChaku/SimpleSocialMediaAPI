using SocialMediaApi.Data.Repositories;
using SocialMediaApi.Models.Dtos;
using SocialMediaApi.Models;

namespace SocialMediaApi.Services;

public class PostService : IPostService
{
    private readonly IPostRepository _repository;
    private readonly ISanitizerService _sanitizer;

    public PostService(IPostRepository repository, ISanitizerService sanitizer)
    {
        _repository = repository;
        _sanitizer = sanitizer;
    }

    public async Task<int> CreatePostAsync(CreatePostDto postDto)
    {
        var post = new Post
        {
            Title = _sanitizer.Sanitize(postDto.Title),
            Body = _sanitizer.Sanitize(postDto.Body),
            AuthorId = postDto.AuthorId
        };

        return await _repository.CreatePostAsync(post);
    }

    public async Task<List<PostDto>> GetAllPostsAsync()
    {
        var posts = await _repository.GetAllPostsAsync();

        return posts.ConvertAll(p => new PostDto
        {
            Id = p.Id,
            Title = p.Title,
            Body = p.Body,
            AuthorId = p.AuthorId
        });
    }

    public async Task LikePostAsync(Like like)
    {
        await _repository.LikePostAsync(like);
    }
}
