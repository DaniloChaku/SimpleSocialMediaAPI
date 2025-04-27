using Dapper;
using SocialMediaApi.Models;

namespace SocialMediaApi.Data.Repositories;

public class PostRepository : IPostRepository
{
    private readonly DapperContext _context;

    public PostRepository(DapperContext context)
    {
        _context = context;
    }

    public async Task<int> CreatePostAsync(Post post)
    {
        var query = "INSERT INTO posts (title, body, author_id) VALUES (@Title, @Body, @AuthorId) RETURNING id;";
        using var connection = _context.CreateConnection();
        return await connection.ExecuteScalarAsync<int>(query, post);
    }

    public async Task<List<Post>> GetAllPostsAsync()
    {
        var query = "SELECT * FROM posts;";
        using var connection = _context.CreateConnection();
        var posts = await connection.QueryAsync<Post>(query);
        return posts.ToList();
    }

    public async Task LikePostAsync(Like like)
    {
        var query = "INSERT INTO likes (user_id, post_id) VALUES (@UserId, @PostId);";
        using var connection = _context.CreateConnection();
        await connection.ExecuteAsync(query, like);
    }
}
