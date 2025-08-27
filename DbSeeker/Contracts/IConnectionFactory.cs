using Npgsql;

namespace DBSeeder.Contracts;

public interface IConnectionFactory
{
    NpgsqlConnection Create();
}