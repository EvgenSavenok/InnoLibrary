using Application.DTO.Booking.ReservationDto;
using Application.UseCases.Booking.Commands.ReservationCommands.UpdateReservation;
using Domain.Entities.Booking;

namespace Application.MappingProfiles.Booking;

public static class ReservationMapper
{
    public static UserBookReservation DtoToEntity(ReservationDto reservationDto)
    {
        var reservationDate = DateTime.Now;
        
        return new UserBookReservation
        {
            Id = Random.Shared.Next(1, int.MaxValue),
            UserId = reservationDto.UserId,
            BookId = reservationDto.BookId,
            ReservationDate = DateTime.Now.ToUniversalTime(),
            DaysBeforeDeadline = reservationDto.DaysBeforeDeadline,
            ReturnDate = reservationDto.DaysBeforeDeadline.HasValue 
                ? reservationDate.AddDays(reservationDto.DaysBeforeDeadline.Value).ToUniversalTime() 
                : null
        };
    }
    
    public static ReservationDto EntityToDto(UserBookReservation reservationEntity)
    {
        return new ReservationDto
        {
            UserId = reservationEntity.UserId,
            BookId = reservationEntity.BookId,
            DaysBeforeDeadline = reservationEntity.DaysBeforeDeadline,
        };
    }

    public static void CommandToEntityInUpdate(UpdateReservationCommand command, ref UserBookReservation reservation)
    {
        reservation.DaysBeforeDeadline = command.ReservationDto.DaysBeforeDeadline;
    }

    public static IEnumerable<ReservationDto> EntitiesToDtos(IEnumerable<UserBookReservation> reservations)
    {
        return reservations.Select(EntityToDto);
    }
}