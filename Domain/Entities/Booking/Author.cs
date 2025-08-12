namespace Domain.Entities.Booking;

public class Author
{
    public Guid Id { get; set; }

    public string FirstName { get; set; }
    public string LastName { get; set; }
    
    public ICollection<Book> BookAuthors { get; set; } = new List<Book>();
}