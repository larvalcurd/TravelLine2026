using BookingApp.Domain.Entities;
using BookingApp.Domain.Interfaces.Repositories;
using BookingApp.Infrastructure.Foundation;
using Microsoft.EntityFrameworkCore;

namespace BookingApp.Infrastructure.Repositories
{
    public class PropertyRepository( BookingDbContext context ) : IPropertyRepository
    {
        private DbSet<Property> Properties => context.Set<Property>();
        public async Task<IReadOnlyCollection<Property>> GetAllAsync()
        {
            return await Properties.AsNoTracking()
                .Include( p => p.RoomTypes )
                .ToListAsync();
        }

        public async Task<Property?> GetByIdAsync( Guid id )
        {
            return await Properties
                .Include( p => p.RoomTypes )
                .FirstOrDefaultAsync( p => p.Id == id );
        }

        public async Task<IReadOnlyCollection<Property>> GetByCityAsync( string city )
        {
            return await Properties.AsNoTracking()
                .Where( p => p.City.ToLower() == city.ToLower() )
                .ToListAsync();
        }

        public Property? GetById( Guid id )
        {
            return Properties
                .Include( p => p.RoomTypes )
                .FirstOrDefault( p => p.Id == id );
        }

        public void Add( Property property )
        {
            Properties.Add( property );
            context.SaveChanges();
        }

        public void Update( Property property )
        {
            Properties.Update( property );
            context.SaveChanges();
        }

        public void Delete( Guid id )
        {
            var entity = Properties.Find( id )
                ?? throw new InvalidOperationException( $"Property with id '{id}' was not found." );

            Properties.Remove( entity );
            context.SaveChanges();
        }
    }
}
