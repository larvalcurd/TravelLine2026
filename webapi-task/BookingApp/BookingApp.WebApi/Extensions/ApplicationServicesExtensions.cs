using BookingApp.Domain.Interfaces.Services;
using BookingApp.Domain.Services;

namespace BookingApp.WebApi.Extensions
{
    public static class DomainServicesExtensions
    {
        public static IServiceCollection AddDomainServices( this IServiceCollection services )
        {
            services.AddScoped<IPropertyService, PropertyService>();
            services.AddScoped<IRoomTypeService, RoomTypeService>();
            services.AddScoped<ISearchService, SearchService>();
            services.AddScoped<IReservationService, ReservationService>();

            return services;
        }
    }
}