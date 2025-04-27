using System.Data;
using Npgsql;

namespace SocialMediaApi.Data;

public class DapperContext
{
    private readonly string _connectionString;

    public DapperContext(IConfiguration config)
    {
        _connectionString = config.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string not found");
    }

    public IDbConnection CreateConnection() => new NpgsqlConnection(_connectionString);
}
