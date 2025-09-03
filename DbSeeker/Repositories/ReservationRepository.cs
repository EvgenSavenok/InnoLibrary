using DBSeeder.Contracts;
using Npgsql;

namespace DBSeeder.Repositories;

public class ReservationRepository(IConnectionFactory factory, BookRepository bookRepository) : BaseRepository(factory)
{
    public override async Task<int> InsertRandomDataAsync()
    {
        var bookId = await bookRepository.InsertRandomDataAsync();
        var rnd = new Random();
        
        var reservationDate = DateTime.UtcNow;
        var daysBeforeDeadline = rnd.Next(1, 30);
        var returnDate = reservationDate.AddDays(daysBeforeDeadline);
            
        var insertReservationCmd = new NpgsqlCommand("""
                                                  INSERT INTO public."UserBookReservations" ("BookId", "UserId", "DaysBeforeDeadline", "ReservationDate", "ReturnDate")
                                                  VALUES (@bookId, @userId, @daysBeforeDeadline, @reservationDate, @returnDate)
                                                  RETURNING "Id"
                                              """);
    
        insertReservationCmd.Parameters.AddWithValue("bookId", bookId);
        insertReservationCmd.Parameters.AddWithValue("userId", rnd.Next(1, 100));
        insertReservationCmd.Parameters.AddWithValue("daysBeforeDeadline", daysBeforeDeadline);
        insertReservationCmd.Parameters.AddWithValue("reservationDate", reservationDate);
        insertReservationCmd.Parameters.AddWithValue("returnDate", returnDate);
        
        int reservationId;
        await using var conn = factory.Create();
        await conn.OpenAsync();
        insertReservationCmd.Connection = conn;
        reservationId = (int)(await insertReservationCmd.ExecuteScalarAsync() ?? throw new Exception("Failed to insert reservation"));
        return reservationId;

    }
}