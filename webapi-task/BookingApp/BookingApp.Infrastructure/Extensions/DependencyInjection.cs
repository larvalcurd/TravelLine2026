using BookingApp.Domain.Interfaces.Repositories;
using BookingApp.Infrastructure.Persistence;
using BookingApp.Infrastructure.Repositories;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BookingApp.Infrastructure.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<BookingDbContext>(options =>
                options.UseSqlite(configuration.GetConnectionString("BookingDatabase")));

            services.AddScoped<IPropertyRepository, EfPropertyRepository>();
            services.AddScoped<IRoomTypeRepository, EfRoomTypeRepository>();
            services.AddScoped<IReservationRepository, EfReservationRepository>();

            return services;
        }
    }

}