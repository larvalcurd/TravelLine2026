using BookingApp.Domain.Entities;
using BookingApp.WebApi.DTOs.Reservations;

using ApiCreateReservationRequest = BookingApp.WebApi.DTOs.Reservations.CreateReservationRequest;
using ApiReservationFilterRequest = BookingApp.WebApi.DTOs.Reservations.ReservationFilterRequest;

using DomainCreateReservationRequest = BookingApp.Domain.Models.CreateReservationRequest;
using DomainReservationFilter = BookingApp.Domain.Models.ReservationFilter;

namespace BookingApp.WebApi.Mappings
{
    public static class ReservationMappingExtensions
    {
        public static DomainCreateReservationRequest ToDomainRequest( this ApiCreateReservationRequest request )
        {
            if ( request == null ) return null!;

            return new DomainCreateReservationRequest
            {
                PropertyId = request.PropertyId,
                RoomTypeId = request.RoomTypeId,
                ArrivalDate = request.ArrivalDate,
                ArrivalTime = request.ArrivalTime!.Value,
                DepartureDate = request.DepartureDate,
                DepartureTime = request.DepartureTime!.Value,
                GuestName = request.GuestName,
                GuestPhoneNumber = request.GuestPhoneNumber,
                GuestCount = request.GuestCount
            };
        }

        public static ReservationResponse ToResponse( this Reservation reservation )
        {
            if ( reservation == null ) return null!;

            return new ReservationResponse
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

        public static DomainReservationFilter ToDomainFilter( this ApiReservationFilterRequest request )

        {
            if ( request == null ) return new DomainReservationFilter { IncludeCanceled = false };

            return new DomainReservationFilter
            {
                PropertyId = request.PropertyId,
                RoomTypeId = request.RoomTypeId,
                ArrivalDateFrom = request.ArrivalDateFrom,
                ArrivalDateTo = request.ArrivalDateTo,
                GuestName = request.GuestName,
                GuestPhoneNumber = request.GuestPhoneNumber,
                IncludeCanceled = request.IncludeCanceled
            };
        }
    }
}
