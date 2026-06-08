using BookingApp.Domain.Entities;
using BookingApp.WebApi.DTOs.RoomTypes;

using ApiCreateRoomRequest = BookingApp.WebApi.DTOs.RoomTypes.CreateRoomTypeRequest;
using ApiUpdateRoomRequest = BookingApp.WebApi.DTOs.RoomTypes.UpdateRoomTypeRequest;

using DomainCreateRoomTypeRequest = BookingApp.Domain.Models.CreateRoomTypeRequest;
using DomainUpdateRoomTypeRequest = BookingApp.Domain.Models.UpdateRoomTypeRequest;

namespace BookingApp.WebApi.Mappings
{
    public static class RoomTypeMappingExtensions
    {
        public static RoomTypeResponse ToResponse( this RoomType rt )
        {
            return new RoomTypeResponse
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

        public static DomainCreateRoomTypeRequest ToDomainRequest( this ApiCreateRoomRequest request )
        {
            return new DomainCreateRoomTypeRequest
            {
                Name = request.Name,
                DailyPrice = request.DailyPrice,
                Currency = request.Currency,
                MinPersonCount = request.MinPersonCount,
                MaxPersonCount = request.MaxPersonCount,
                TotalRoomsCount = request.TotalRoomsCount,
                Services = request.Services?.ToList() ?? [],
                Amenities = request.Amenities?.ToList() ?? []
            };
        }

        public static DomainUpdateRoomTypeRequest ToDomainRequest( this ApiUpdateRoomRequest request )

        {
            return new DomainUpdateRoomTypeRequest
            {
                Name = request.Name,
                DailyPrice = request.DailyPrice,
                Currency = request.Currency,
                MinPersonCount = request.MinPersonCount,
                MaxPersonCount = request.MaxPersonCount,
                TotalRoomsCount = request.TotalRoomsCount,
                Services = request.Services?.ToList() ?? [],
                Amenities = request.Amenities?.ToList() ?? []
            };
        }
    }
}
