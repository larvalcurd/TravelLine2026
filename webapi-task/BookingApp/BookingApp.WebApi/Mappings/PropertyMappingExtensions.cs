using BookingApp.Domain.Entities;
using BookingApp.WebApi.DTOs.Properties;

using DomainCreatePropertyReq = BookingApp.Domain.Models.CreatePropertyRequest;
using DomainUpdatePropertyReq = BookingApp.Domain.Models.UpdatePropertyRequest;


namespace BookingApp.WebApi.Mappings
{
    public static class PropertyMappingExtensions
    {
        public static PropertyResponse ToResponse( this Property property )
        {
            if ( property == null ) return null!;

            return new PropertyResponse
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

        public static DomainCreatePropertyReq ToDomainRequest( this CreatePropertyRequest apiRequest )
        {
            return new DomainCreatePropertyReq
            {
                Name = apiRequest.Name,
                Country = apiRequest.Country,
                City = apiRequest.City,
                Address = apiRequest.Address,
                Latitude = apiRequest.Latitude,
                Longitude = apiRequest.Longitude
            };
        }

        public static DomainUpdatePropertyReq ToDomainRequest( this UpdatePropertyRequest apiRequest )
        {
            return new DomainUpdatePropertyReq
            {
                Name = apiRequest.Name,
                Country = apiRequest.Country,
                City = apiRequest.City,
                Address = apiRequest.Address,
                Latitude = apiRequest.Latitude,
                Longitude = apiRequest.Longitude
            };
        }
    }
}