using Application.Contracts.Repository.Booking;
using Application.DTO.Booking.ReservationDto;
using Application.MappingProfiles.Booking;
using Application.RequestFeatures;
using MediatR;

namespace Application.UseCases.Booking.Queries.ReservationQueries.GetAllReservationsOfUser;

public class GetAllReservationsOfUserQueryHandler(
    IUnitOfWork unitOfWork) : IRequestHandler<GetAllReservationsOfUserQuery, PagedResult<ReservationDto>>
{
    public async Task<PagedResult<ReservationDto>> Handle(GetAllReservationsOfUserQuery request, CancellationToken cancellationToken)
    {
        var reservationsPaged = await unitOfWork.ReservationRepository.GetAllReservationsAsync(request.Parameters, cancellationToken);

        return new PagedResult<ReservationDto>
        {
            Items = ReservationMapper.EntitiesToDtos(reservationsPaged.Items),
            TotalCount = reservationsPaged.TotalCount,
            PageNumber = reservationsPaged.PageNumber,
            PageSize = reservationsPaged.PageSize
        };
    }
}