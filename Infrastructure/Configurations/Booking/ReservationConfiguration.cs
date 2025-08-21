using Domain.Entities.Booking;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.Booking;

public class ReservationConfiguration : IEntityTypeConfiguration<UserBookReservation>
{
    public void Configure(EntityTypeBuilder<UserBookReservation> builder)
    {
        builder.HasKey(reservation => reservation.Id);
        builder.Property(reservation => reservation.UserId);
        builder.Property(reservation => reservation.BookId).IsRequired();
        builder.Property(reservation => reservation.DaysBeforeDeadline).IsRequired();
        builder.Property(reservation => reservation.ReservationDate);
        builder.Property(reservation => reservation.ReturnDate);
    }
}