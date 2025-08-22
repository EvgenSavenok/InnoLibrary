namespace Application.DTO.Booking.ReservationDto;

public record ReservationDto
{
    public int BookId { get; set; }
    public int? UserId { get; set; }
    
    public int? DaysBeforeDeadline { get; set; }
}