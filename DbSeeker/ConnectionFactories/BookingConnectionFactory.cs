using DBSeeder.Contracts;
using Npgsql;

namespace DBSeeder.ConnectionFactories;

public class BookingConnectionFactory(IConfiguration config) : IBookingConnectionFactory
{
    private readonly string? _connectionString = config.GetConnectionString("bookingConnection");

    public NpgsqlConnection Create() => new(_connectionString);
}