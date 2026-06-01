using BookingApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookingApp.Infrastructure.Persistence
{
    public class BookingDbContext( DbContextOptions<BookingDbContext> options ) : DbContext( options )
    {
        public DbSet<Property> Properties => Set<Property>();
        public DbSet<RoomType> RoomTypes => Set<RoomType>();
        public DbSet<Reservation> Reservations => Set<Reservation>();

        protected override void OnModelCreating( ModelBuilder modelBuilder )
        {
            base.OnModelCreating( modelBuilder );

            modelBuilder.ApplyConfigurationsFromAssembly( typeof( BookingDbContext ).Assembly );
        }
    }
}