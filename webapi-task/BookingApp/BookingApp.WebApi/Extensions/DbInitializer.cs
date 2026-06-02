using BookingApp.Infrastructure.Persistence;
using BookingApp.Infrastructure.Persistence.Seed;
using Microsoft.EntityFrameworkCore;

namespace BookingApp.WebApi.Extensions
{
    public static class DbInitializer
    {
        public static void InitializeDatabase( this WebApplication app )
        {
            using var scope = app.Services.CreateScope();

            var dbContext = scope.ServiceProvider.GetRequiredService<BookingDbContext>();

            dbContext.Database.Migrate();
            BookingDbSeeder.Seed( dbContext );
        }
    }
}