using DBSeeder.Contracts;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace DBSeeder.ConnectionFactories;

public class UserConnectionFactory(IConfiguration config) : IUserConnectionFactory
{
    private readonly string? _connectionString = config.GetConnectionString("userConnection");

    public NpgsqlConnection Create() => new(_connectionString);
}