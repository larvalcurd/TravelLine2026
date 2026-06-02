using BookingApp.Domain.Entities;
using BookingApp.Domain.Interfaces.Repositories;
using BookingApp.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BookingApp.Infrastructure.Repositories
{
    public class PropertyRepository( BookingDbContext context ) : IPropertyRepository
    {
        private DbSet<Property> Properties => context.Properties;
        public IReadOnlyCollection<Property> GetAll()
        {
            return Properties.AsNoTracking()
                .Include( p => p.RoomTypes )
                .ToList();
        }

        public Property? GetById( Guid id )
        {
            return Properties
                .Include( p => p.RoomTypes )
                .FirstOrDefault( p => p.Id == id );
        }

        public IReadOnlyCollection<Property> GetByCity( string city )
        {
            return Properties.AsNoTracking()
                .Where( p => p.City.ToLower() == city.ToLower() )
                .ToList();
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
