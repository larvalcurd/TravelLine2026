using BookingApp.Domain.Entities;
using BookingApp.WebApi.DTOs.Properties;

namespace BookingApp.WebApi.Mappings
{
    public static class PropertyMappingExtensions
    {
        public static PropertyDto ToDto(this Property property)
        {
            if (property == null) return null!;

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

        public static Property ToEntity(this CreatePropertyDto dto)
        {
            if (dto == null) return null!;

            return new Property
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Country = dto.Country,
                City = dto.City,
                Address = dto.Address,
                Latitude = dto.Latitude,
                Longitude = dto.Longitude
            };
        }

        public static Property ToEntity(this UpdatePropertyDto dto, Guid id)
        {
            if (dto == null) return null!;

            return new Property
            {
                Id = id,
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
