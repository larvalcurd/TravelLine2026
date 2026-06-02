using BookingApp.Domain.Entities;
using BookingApp.Domain.Interfaces.Repositories;
using BookingApp.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BookingApp.Infrastructure.Repositories
{
    public class RoomTypeRepository( BookingDbContext context ) : IRoomTypeRepository
    {
        private DbSet<RoomType> RoomTypes => context.RoomTypes;

        public IReadOnlyCollection<RoomType> GetAll()
        {
            return RoomTypes.AsNoTracking().ToList();
        }

        public IReadOnlyCollection<RoomType> GetByPropertyId( Guid propertyId )
        {
            return RoomTypes.AsNoTracking()
                .Where( rt => rt.PropertyId == propertyId )
                .ToList();
        }

        public IReadOnlyCollection<RoomType> GetByPropertyIds( IEnumerable<Guid> propertyIds )
        {
            var idSet = propertyIds.ToHashSet();

            return RoomTypes.AsNoTracking()
                .Where( rt => idSet.Contains( rt.PropertyId ) )
                .ToList();
        }

        public IReadOnlyCollection<RoomType> GetCandidates(
            IEnumerable<Guid> propertyIds,
            int guests,
            decimal? maxPrice )
        {
            var idSet = propertyIds.ToHashSet();

            var query = RoomTypes
                .AsNoTracking()
                .Where( rt => idSet.Contains( rt.PropertyId ) )
                .Where( rt => guests >= rt.MinPersonCount && guests <= rt.MaxPersonCount );

            if ( maxPrice.HasValue )
            {
                query = query.Where( rt => rt.DailyPrice <= maxPrice.Value );
            }

            return query.ToList();
        }

        public bool HasRoomTypesForProperty( Guid propertyId )
        {
            return RoomTypes.Any( rt => rt.PropertyId == propertyId );
        }

        public RoomType? GetById( Guid id )
        {
            return RoomTypes.FirstOrDefault( rt => rt.Id == id );
        }

        public void Add( RoomType roomType )
        {
            RoomTypes.Add( roomType );
            context.SaveChanges();
        }

        public void Update( RoomType roomType )
        {
            RoomTypes.Update( roomType );
            context.SaveChanges();
        }

        public void Delete( Guid id )
        {
            var entity = RoomTypes.Find( id )
                ?? throw new InvalidOperationException( $"RoomType with id '{id}' was not found." );

            RoomTypes.Remove( entity );
            context.SaveChanges();
        }
    }
}