namespace Domain.Entities.Booking;

public class UserBookReservations
{
    public Guid Id { get; set; }
    public Guid BookId { get; set; }
    public Guid UserId { get; set; }
    
    public DateTime ReservationDate { get; set; }
    public DateTime ReturnDate { get; set; }
}