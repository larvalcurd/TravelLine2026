using BookingApp.Domain.Entities;
using BookingApp.Domain.Models;
using BookingApp.WebApi.DTOs.RoomTypes;

namespace BookingApp.WebApi.Mappings
{
    public static class RoomTypeMappingExtensions
    {
        public static RoomTypeDto ToDto( this RoomType rt )
        {
            if ( rt == null ) return null!;

            return new RoomTypeDto
            {
                Id = rt.Id,
                PropertyId = rt.PropertyId,
                Name = rt.Name,
                DailyPrice = rt.DailyPrice,
                Currency = rt.Currency,
                MinPersonCount = rt.MinPersonCount,
                MaxPersonCount = rt.MaxPersonCount,
                TotalRoomsCount = rt.TotalRoomsCount,
                Services = rt.Services?.ToList() ?? [],
                Amenities = rt.Amenities?.ToList() ?? []
            };
        }

        public static CreateRoomTypeRequest ToCreateRequest( this CreateRoomTypeDto dto )
        {
            if ( dto == null ) return null!;

            return new CreateRoomTypeRequest
            {
                Name = dto.Name,
                DailyPrice = dto.DailyPrice,
                Currency = dto.Currency,
                MinPersonCount = dto.MinPersonCount,
                MaxPersonCount = dto.MaxPersonCount,
                TotalRoomsCount = dto.TotalRoomsCount,
                Services = dto.Services?.ToList() ?? [],
                Amenities = dto.Amenities?.ToList() ?? []
            };
        }

        public static UpdateRoomTypeRequest ToUpdateRequest( this UpdateRoomTypeDto dto )
        {
            if ( dto == null ) return null!;

            return new UpdateRoomTypeRequest
            {
                Name = dto.Name,
                DailyPrice = dto.DailyPrice,
                Currency = dto.Currency,
                MinPersonCount = dto.MinPersonCount,
                MaxPersonCount = dto.MaxPersonCount,
                TotalRoomsCount = dto.TotalRoomsCount,
                Services = dto.Services?.ToList() ?? [],
                Amenities = dto.Amenities?.ToList() ?? []
            };
        }
    }
}
