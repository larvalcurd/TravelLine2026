using BookingApp.Domain.Interfaces.Repositories;
using BookingApp.Infrastructure.Foundation;
using BookingApp.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BookingApp.Infrastructure.Extensions
{
    public static class ServiceBinding
    {
        public static IServiceCollection AddInfrastructure( this IServiceCollection services, IConfiguration configuration )
        {
            services.AddDbContext<BookingDbContext>( options =>
                options.UseSqlite( configuration.GetConnectionString( "BookingDatabase" ) ) );

            services.AddScoped<IPropertyRepository, PropertyRepository>();
            services.AddScoped<IRoomTypeRepository, RoomTypeRepository>();
            services.AddScoped<IReservationRepository, ReservationRepository>();

            return services;
        }
    }
}