using BookingApp.Domain.Interfaces.Services;
using BookingApp.Domain.Services;
using Microsoft.Extensions.DependencyInjection;

namespace BookingApp.Domain.Extensions
{
    public static class ServiceBinding
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