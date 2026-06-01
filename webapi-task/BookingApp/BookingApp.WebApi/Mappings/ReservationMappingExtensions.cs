using BookingApp.Domain.Entities;
using BookingApp.Domain.Models;
using BookingApp.WebApi.DTOs.Reservations;

namespace BookingApp.WebApi.Mappings
{
    public static class ReservationMappingExtensions
    {
        public static CreateReservationRequest ToRequest( this CreateReservationDto dto )
        {
            if ( dto == null ) return null!;

            return new CreateReservationRequest
            {
                PropertyId = dto.PropertyId,
                RoomTypeId = dto.RoomTypeId,
                ArrivalDate = dto.ArrivalDate,
                ArrivalTime = dto.ArrivalTime!.Value,
                DepartureDate = dto.DepartureDate,
                DepartureTime = dto.DepartureTime!.Value,
                GuestName = dto.GuestName,
                GuestPhoneNumber = dto.GuestPhoneNumber,
                GuestCount = dto.GuestCount
            };
        }

        public static ReservationDto ToDto( this Reservation reservation )
        {
            if ( reservation == null ) return null!;

            return new ReservationDto
            {
                Id = reservation.Id,
                PropertyId = reservation.PropertyId,
                RoomTypeId = reservation.RoomTypeId,
                ArrivalDate = reservation.ArrivalDate,
                DepartureDate = reservation.DepartureDate,
                ArrivalTime = reservation.ArrivalTime,
                DepartureTime = reservation.DepartureTime,
                GuestName = reservation.GuestName,
                GuestPhoneNumber = reservation.GuestPhoneNumber,
                GuestCount = reservation.GuestCount,
                Total = reservation.Total,
                Currency = reservation.Currency,
                IsCanceled = reservation.IsCanceled
            };
        }

        public static ReservationFilter ToFilter( this ReservationFilterDto dto )
        {
            if ( dto == null ) return new ReservationFilter { IncludeCanceled = false };

            return new ReservationFilter
            {
                PropertyId = dto.PropertyId,
                RoomTypeId = dto.RoomTypeId,
                ArrivalDateFrom = dto.ArrivalDateFrom,
                ArrivalDateTo = dto.ArrivalDateTo,
                GuestName = dto.GuestName,
                GuestPhoneNumber = dto.GuestPhoneNumber,
                IncludeCanceled = dto.IncludeCanceled
            };
        }
    }
}
