using DBSeeder.Contracts;
using Npgsql;

namespace DBSeeder.Repositories;

public class BookRepository(IConnectionFactory factory, AuthorRepository authorRepository) : BaseRepository(factory)
{
    public override async Task<int> InsertRandomDataAsync()
    {
        var rnd = new Random();

        var insertBookCmd = new NpgsqlCommand("""
                                                  INSERT INTO public."Books" ("ISBN", "BookTitle", "GenreType", "Description", "Amount")
                                                  VALUES (@isbn, @title, @genre, @desc, @amount)
                                                  RETURNING "Id"
                                              """);


        string isbn = string.Concat(Enumerable.Range(0, 13).Select(_ => rnd.Next(0, 10).ToString()));

        insertBookCmd.Parameters.AddWithValue("isbn", isbn);
        insertBookCmd.Parameters.AddWithValue("title", $"Book {rnd.Next(1, 100)}");
        insertBookCmd.Parameters.AddWithValue("genre", rnd.Next(1, 3));
        insertBookCmd.Parameters.AddWithValue("desc", "Random");
        insertBookCmd.Parameters.AddWithValue("amount", (short)rnd.Next(1, 50));
        
        int bookId;
        await using (var conn = factory.Create())
        {
            await conn.OpenAsync();
            insertBookCmd.Connection = conn;
            bookId = (int)(await insertBookCmd.ExecuteScalarAsync() ?? throw new Exception("Failed to insert book"));
        }
        
        int authorsCount = rnd.Next(1, 4);
        var authorIds = new List<int>();
        for (int i = 0; i < authorsCount; i++)
        {
            var authorId = await authorRepository.InsertRandomDataAsyncReturnId();
            authorIds.Add(authorId);
        }

        foreach (var authorId in authorIds)
        {
            var cmdLink = new NpgsqlCommand("""
                                                INSERT INTO public."BookAuthor" ("BookId", "AuthorId")
                                                VALUES (@bookId, @authorId)
                                            """);
            cmdLink.Parameters.AddWithValue("bookId", bookId);
            cmdLink.Parameters.AddWithValue("authorId", authorId);
            await ExecuteCommandAsync(cmdLink);
        }

        return bookId;
    }
}