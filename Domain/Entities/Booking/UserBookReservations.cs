namespace Domain.Entities.Booking;

public class UserBookReservations
{
    public int Id { get; set; }
    public int BookId { get; set; }
    public int? UserId { get; set; }
    
    public DateTime? ReservationDate { get; set; }
    public DateTime? ReturnDate { get; set; }
}