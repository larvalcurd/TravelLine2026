using BookingApp.Domain.Models;
using BookingApp.WebApi.DTOs.Search;

namespace BookingApp.WebApi.Mappings
{
    public static class SearchMappingExtensions
    {
        public static SearchAvailabilityCriteria ToCriteria( this SearchAvailabilityQueryDto dto )
        {
            if ( dto == null ) return null!;

            return new SearchAvailabilityCriteria
            {
                City = dto.City,
                ArrivalDate = dto.ArrivalDate,
                DepartureDate = dto.DepartureDate,
                Guests = dto.Guests,
                MaxPrice = dto.MaxPrice,
            };

        }
        public static SearchAvailabilityResultDto ToDto( this AvailableRoomOption option )
        {
            if ( option == null ) return null!;

            return new SearchAvailabilityResultDto
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