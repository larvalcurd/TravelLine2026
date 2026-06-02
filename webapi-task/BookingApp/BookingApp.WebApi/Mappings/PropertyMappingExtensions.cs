using BookingApp.Domain.Entities;
using BookingApp.Domain.Models;
using BookingApp.WebApi.DTOs.Properties;

namespace BookingApp.WebApi.Mappings
{
    public static class PropertyMappingExtensions
    {
        public static PropertyDto ToDto( this Property property )
        {
            if ( property == null ) return null!;

            return new PropertyDto
            {
                Id = property.Id,
                Name = property.Name,
                Country = property.Country,
                City = property.City,
                Address = property.Address,
                Latitude = property.Latitude,
                Longitude = property.Longitude
            };
        }

        public static CreatePropertyRequest ToCreateRequest( this CreatePropertyDto dto )
        {
            return new CreatePropertyRequest
            {
                Name = dto.Name,
                Country = dto.Country,
                City = dto.City,
                Address = dto.Address,
                Latitude = dto.Latitude,
                Longitude = dto.Longitude
            };
        }

        public static UpdatePropertyRequest ToUpdateRequest( this UpdatePropertyDto dto )
        {
            return new UpdatePropertyRequest
            {
                Name = dto.Name,
                Country = dto.Country,
                City = dto.City,
                Address = dto.Address,
                Latitude = dto.Latitude,
                Longitude = dto.Longitude
            };
        }
    }
}
