using System.Text.Json.Serialization;
using Domain.Enums.Booking;

namespace Domain.Entities.Booking;

public class Book
{
    public int Id { get; set; }
    public int? UserId { get; set; }
    public int AuthorId { get; set; }
    
    public string ISBN { get; set; }
    public string BookTitle { get; set; }
    public GenreType GenreType { get; set; }
    public string Description { get; set; }
    public Int16 Amount { get; set; }
    
    public ICollection<Author> BookAuthors { get; set; } = new List<Author>();
    public ICollection<UserBookReservations> BookReservations { get; set; } = new List<UserBookReservations>();
}