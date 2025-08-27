using DBSeeder.Contracts;
using DBSeeder.Repositories;

namespace DbSeeder.Services;

public class SeedService(IConnectionFactory factory)
{
    private readonly BookRepository _bookRepository = new(factory, new AuthorRepository(factory));
    private readonly AuthorRepository _authorRepository = new(factory);
    private readonly ReservationRepository _reservationRepository = new(factory, new BookRepository(factory, new AuthorRepository(factory)));
    
    public async Task SeedAuthorsAsync(int count)
    {
        for (int i = 0; i < count; i++)
        {
            await _authorRepository.InsertRandomDataAsync();
        }
    }

    public async Task SeedBooksAsync(int count)
    {
        for (int i = 0; i < count; i++)
        {
            await _bookRepository.InsertRandomDataAsync();
        }
    }
    
    public async Task SeedReservationsAsync(int count)
    {
        for (int i = 0; i < count; i++)
        {
            await _reservationRepository.InsertRandomDataAsync();
        }
    }
}