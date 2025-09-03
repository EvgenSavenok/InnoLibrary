using DBSeeder.Contracts;
using Npgsql;

namespace DBSeeder.Repositories;

public class AuthorRepository(IConnectionFactory factory) : BaseRepository(factory)
{
    public override async Task<int> InsertRandomDataAsync()
    {
        var rnd = new Random();

        var cmd = new NpgsqlCommand("""
                                        INSERT INTO public."Authors" ("FirstName", "LastName")
                                        VALUES (@fname, @lname)
                                    """);

        cmd.Parameters.AddWithValue("fname", $"Author{rnd.Next(1, 100)}");
        cmd.Parameters.AddWithValue("lname", $"Lastname{rnd.Next(1, 100)}");

        var rows = await ExecuteCommandAsync(cmd);
        return rows;
    }
    
    public async Task<int> InsertRandomDataAsyncReturnId()
    {
        var rnd = new Random();
        string fname = $"Author{rnd.Next(1,100)}";
        string lname = $"Lastname{rnd.Next(1,100)}";

        var cmd = new NpgsqlCommand("""
                                        INSERT INTO public."Authors" ("FirstName", "LastName")
                                        VALUES (@fname, @lname)
                                        RETURNING "AuthorId"
                                    """);
        cmd.Parameters.AddWithValue("fname", fname);
        cmd.Parameters.AddWithValue("lname", lname);

        await using var conn = factory.Create();
        await conn.OpenAsync();
        cmd.Connection = conn;
        var id = (int)await cmd.ExecuteScalarAsync();
        return id;
    }
}