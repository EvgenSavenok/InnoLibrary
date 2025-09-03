using DBSeeder.Contracts;
using Npgsql;

namespace DBSeeder.Repositories;

public abstract class BaseRepository(IConnectionFactory factory) 
{
    protected async Task<int> ExecuteCommandAsync(NpgsqlCommand cmd)
    {
        await using var conn = factory.Create();
        await conn.OpenAsync();
        cmd.Connection = conn;
        var rows = await cmd.ExecuteNonQueryAsync();
        return rows;
    }

    public abstract Task<int> InsertRandomDataAsync();
}