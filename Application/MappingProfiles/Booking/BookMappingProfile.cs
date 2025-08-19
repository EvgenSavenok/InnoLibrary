using Application.DTO.Booking.BookDto;
using Application.UseCases.Booking.Commands.BookCommands.CreateBook;
using Domain.Entities.Booking;

namespace Application.MappingProfiles.Booking;

public static class BookMapper
{
    public static Book CommandToEntity(CreateBookCommand command)
    {
        return new Book
        {
            Id = Random.Shared.Next(1, int.MaxValue),
            UserId = command.BookDto.UserId,
            ISBN = command.BookDto.ISBN,
            BookTitle = command.BookDto.BookTitle,
            Description = command.BookDto.Description,
            GenreType = command.BookDto.GenreType,
            Amount = command.BookDto.Amount,
            BookAuthors = command.BookDto.BookAuthors
                .Select(authorDto => new Author
                {
                    Id = authorDto.Id,
                    FirstName = authorDto.FirstName,
                    LastName = authorDto.LastName
                })
                .ToList(),
            BookReservations = command.BookDto.BookReservations
                .Select(reservationDto => new UserBookReservations
                {
                    Id = reservationDto.Id,
                    UserId = reservationDto.UserId,
                    BookId = reservationDto.BookId,
                    ReservationDate = DateTime.UtcNow
                })
                .ToList()
        };
    }

    public static Book DtoToEntity(BookDto bookDto)
    {
        return new Book
        {
            Id = bookDto.Id,
            UserId = bookDto.UserId,
            ISBN = bookDto.ISBN,
            BookTitle = bookDto.BookTitle,
            Description = bookDto.Description,
            GenreType = bookDto.GenreType,
            Amount = bookDto.Amount,
            BookAuthors = bookDto.BookAuthors
                .Select(authorDto => new Author
                {
                    Id = authorDto.Id,
                    FirstName = authorDto.FirstName,
                    LastName = authorDto.LastName
                })
                .ToList(),
            BookReservations = bookDto.BookReservations
                .Select(reservationDto => new UserBookReservations
                {
                    Id = reservationDto.Id,
                    UserId = reservationDto.UserId,
                    BookId = reservationDto.BookId,
                    ReservationDate = DateTime.UtcNow
                })
                .ToList()
        };
    }
    
    public static BookDto EntityToDto(Book book)
    {
        return new BookDto
        {
            UserId = book.UserId,
            ISBN = book.ISBN,
            BookTitle = book.BookTitle,
            Description = book.Description,
            GenreType = book.GenreType,
            Amount = book.Amount,
            BookAuthors = book.BookAuthors
                .Select(authorDto => new Author
                {
                    Id = authorDto.Id,
                    FirstName = authorDto.FirstName,
                    LastName = authorDto.LastName
                })
                .ToList(),
            BookReservations = book.BookReservations
                .Select(reservationDto => new UserBookReservations
                {
                    Id = reservationDto.Id,
                    UserId = reservationDto.UserId,
                    BookId = reservationDto.BookId,
                    ReservationDate = DateTime.UtcNow
                })
                .ToList()
        };
    }
    
    public static IEnumerable<BookDto> EntitiesToDtos(IEnumerable<Book> books)
    {
        return books.Select(EntityToDto);
    }
}