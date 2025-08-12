using System.Text.Json.Serialization;
using Domain.Enums.Booking;

namespace Domain.Entities.Booking;

public class Book
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid AuthorId { get; set; }
    
    public string ISBN { get; set; }
    public string BookTitle { get; set; }
    public Genre Genre { get; set; }
    public string Description { get; set; }
    public Int16 Amount { get; set; }
    
    public ICollection<Author> BookAuthors { get; set; } = new List<Author>();
    public ICollection<UserBookReservations> BookReservations { get; set; } = new List<UserBookReservations>();
}