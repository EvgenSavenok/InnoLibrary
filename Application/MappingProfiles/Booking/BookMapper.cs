using Application.DTO.Booking.AuthorDto;
using Application.DTO.Booking.BookDto;
using Application.UseCases.Booking.Commands.BookCommands.CreateBook;
using Application.UseCases.Booking.Commands.BookCommands.UpdateBook;
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
            BookReservations = command.BookDto.BookReservations
                .Select(reservationDto => new UserBookReservation
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
                .Select(authorDto => new BookAuthorDto()
                {
                    AuthorId = authorDto.AuthorId,
                    FirstName = authorDto.FirstName,
                    LastName = authorDto.LastName
                })
                .ToList(),
            BookReservations = book.BookReservations
                .Select(reservationDto => new UserBookReservation
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

    public static void CommandToEntityInUpdate(UpdateBookCommand command, ref Book book)
    {
        book.Id = command.BookDto.Id;
        book.BookTitle = command.BookDto.BookTitle;
        book.ISBN = command.BookDto.ISBN;
        book.Description = command.BookDto.Description;
        book.Amount = command.BookDto.Amount;
        book.GenreType = command.BookDto.GenreType;
        book.BookReservations = command.BookDto.BookReservations
            .Select(reservationDto => new UserBookReservation
            {
                Id = reservationDto.Id,
                UserId = reservationDto.UserId,
                BookId = reservationDto.BookId,
                ReservationDate = DateTime.UtcNow
            })
            .ToList();
    }
}