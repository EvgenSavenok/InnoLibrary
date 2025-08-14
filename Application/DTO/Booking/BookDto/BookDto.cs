using Domain.Entities.Booking;
using Domain.Enums.Booking;

namespace Application.DTO.Booking.BookDto;

public record BookDto
{
    public int Id { get; set; }
    public int? UserId { get; set; }
    
    public string ISBN { get; set; }
    public string BookTitle { get; set; }
    public GenreType GenreType { get; set; }
    public string Description { get; set; }
    public Int16 Amount { get; set; }
    
    public ICollection<Author> BookAuthors { get; set; } = new List<Author>();
    public ICollection<UserBookReservations> BookReservations { get; set; } = new List<UserBookReservations>();
}