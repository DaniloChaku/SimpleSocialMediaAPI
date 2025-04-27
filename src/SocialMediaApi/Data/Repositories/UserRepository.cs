using Dapper;
using SocialMediaApi.Models;

namespace SocialMediaApi.Data.Repositories;

public class UserRepository : IUserRepository
{
    private readonly DapperContext _context;

    public UserRepository(DapperContext context)
    {
        _context = context;
    }

    public async Task<int> CreateUserAsync(User user)
    {
        var query = "INSERT INTO users (username) VALUES (@Username) RETURNING id;";
        using var connection = _context.CreateConnection();
        return await connection.ExecuteScalarAsync<int>(query, user);
    }

    public async Task FollowUserAsync(Follow follow)
    {
        var query = "INSERT INTO follows (follower_id, following_id) VALUES (@FollowerId, @FollowingId);";
        using var connection = _context.CreateConnection();
        await connection.ExecuteAsync(query, follow);
    }
}