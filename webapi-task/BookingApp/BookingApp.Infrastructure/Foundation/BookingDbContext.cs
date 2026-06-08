using BookingApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookingApp.Infrastructure.Foundation
{
    public class BookingDbContext( DbContextOptions<BookingDbContext> options ) : DbContext( options )
    {
        protected override void OnModelCreating( ModelBuilder modelBuilder )
        {
            base.OnModelCreating( modelBuilder );

            modelBuilder.ApplyConfigurationsFromAssembly( typeof( BookingDbContext ).Assembly );
        }
    }
}