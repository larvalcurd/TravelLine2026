using BookingApp.Domain.Models;
using BookingApp.WebApi.DTOs.Search;

namespace BookingApp.WebApi.Mappings
{
    public static class SearchMappingExtensions
    {
        public static SearchAvailabilityCriteria ToDomainCriteria( this SearchAvailabilityRequest request )
        {
            return new SearchAvailabilityCriteria
            {
                City = request.City,
                ArrivalDate = request.ArrivalDate,
                DepartureDate = request.DepartureDate,
                Guests = request.Guests,
                MaxPrice = request.MaxPrice,
            };

        }

        public static SearchAvailabilityResponse ToResponse( this AvailableRoomOption option )
        {
            return new SearchAvailabilityResponse
            {
                PropertyId = option.PropertyId,
                PropertyName = option.PropertyName,
                Country = option.Country,
                City = option.City,
                Address = option.Address,
                Latitude = option.Latitude,
                Longitude = option.Longitude,

                RoomTypeId = option.RoomTypeId,
                RoomTypeName = option.RoomTypeName,
                DailyPrice = option.DailyPrice,
                Currency = option.Currency,
                MinPersonCount = option.MinPersonCount,
                MaxPersonCount = option.MaxPersonCount,

                AvailableRoomsCount = option.AvailableRoomsCount,
                Nights = option.Nights,
                TotalPrice = option.TotalPrice,

                Services = option.Services?.ToList() ?? [],
                Amenities = option.Amenities?.ToList() ?? []
            };
        }
    }
}