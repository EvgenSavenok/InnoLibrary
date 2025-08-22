namespace Domain.Entities.Booking;

public class UserBookReservation
{
    public int Id { get; set; }
    public int BookId { get; set; }
    public int? UserId { get; set; }
    
    public int? DaysBeforeDeadline { get; set; }
    public DateTime? ReservationDate { get; set; }
    public DateTime? ReturnDate { get; set; }
}