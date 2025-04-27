using Dapper;

namespace SocialMediaApi.Data;

public class MigrationService
{
    private readonly DapperContext _context;
    private readonly ILogger<MigrationService> _logger;

    public MigrationService(DapperContext context, ILogger<MigrationService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task InitializeAsync()
    {
        using var connection = _context.CreateConnection();

        var createTablesQuery = @"
        CREATE TABLE IF NOT EXISTS users (
            id SERIAL PRIMARY KEY,
            username VARCHAR(50) NOT NULL UNIQUE
        );

        CREATE TABLE IF NOT EXISTS posts (
            id SERIAL PRIMARY KEY,
            title VARCHAR(100) NOT NULL,
            body TEXT NOT NULL,
            author_id INT REFERENCES users(id) ON DELETE CASCADE
        );

        CREATE TABLE IF NOT EXISTS follows (
            follower_id INT REFERENCES users(id) ON DELETE CASCADE,
            following_id INT REFERENCES users(id) ON DELETE CASCADE,
            PRIMARY KEY (follower_id, following_id)
        );

        CREATE TABLE IF NOT EXISTS likes (
            user_id INT REFERENCES users(id) ON DELETE CASCADE,
            post_id INT REFERENCES posts(id) ON DELETE CASCADE,
            PRIMARY KEY (user_id, post_id)
        );
        ";

        await connection.ExecuteAsync(createTablesQuery);

        _logger.LogInformation("Database migration completed successfully.");
    }
}
