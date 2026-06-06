using BookingApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookingApp.Infrastructure.Foundation
{
    public class BookingDbContext( DbContextOptions<BookingDbContext> options ) : DbContext( options )
    {
        public DbSet<Property> Properties { get; set; }
        public DbSet<RoomType> RoomTypes { get; set; }
        public DbSet<Reservation> Reservations { get; set; }

        protected override void OnModelCreating( ModelBuilder modelBuilder )
        {
            base.OnModelCreating( modelBuilder );

            modelBuilder.ApplyConfigurationsFromAssembly( typeof( BookingDbContext ).Assembly );
        }
    }
}