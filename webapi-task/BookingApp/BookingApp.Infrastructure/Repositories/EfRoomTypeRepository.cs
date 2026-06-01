using BookingApp.Domain.Entities;
using BookingApp.Domain.Interfaces.Repositories;
using BookingApp.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BookingApp.Infrastructure.Repositories
{
    public class EfRoomTypeRepository( BookingDbContext context ) : IRoomTypeRepository
    {
        private readonly BookingDbContext _context = context;

        public IReadOnlyCollection<RoomType> GetAll()
        {
            return _context.RoomTypes.AsNoTracking().ToList();
        }

        public IReadOnlyCollection<RoomType> GetByPropertyId( Guid propertyId )
        {
            return _context.RoomTypes.AsNoTracking().Where( rt => rt.PropertyId == propertyId ).ToList();
        }

        public IReadOnlyCollection<RoomType> GetByPropertyIds( IEnumerable<Guid> propertyIds )
        {
            var idSet = propertyIds.ToHashSet();

            return _context.RoomTypes.AsNoTracking().Where( rt => idSet.Contains( rt.PropertyId ) ).ToList();
        }

        public IReadOnlyCollection<RoomType> GetCandidates(
            IEnumerable<Guid> propertyIds,
            int guests,
            decimal? maxPrice )
        {
            var idSet = propertyIds.ToHashSet();

            var query = _context.RoomTypes
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
            return _context.RoomTypes.Any( rt => rt.PropertyId == propertyId );
        }

        public RoomType? GetById( Guid id )
        {
            return _context.RoomTypes
                .AsNoTracking()
                .FirstOrDefault( rt => rt.Id == id );
        }

        public void Add( RoomType roomType )
        {
            _context.RoomTypes.Add( roomType );
            _context.SaveChanges();
        }

        public void Update( RoomType roomType )
        {
            var existing = _context.RoomTypes.Find( roomType.Id )
                ?? throw new InvalidOperationException( $"RoomType with id '{roomType.Id}' was not found." );

            _context.Entry( existing ).CurrentValues.SetValues( roomType );

            existing.Services = roomType.Services.ToList();
            existing.Amenities = roomType.Amenities.ToList();

            _context.SaveChanges();
        }

        public void Delete( Guid id )
        {
            var entity = _context.RoomTypes.Find( id )
                ?? throw new InvalidOperationException( $"RoomType with id '{id}' was not found." );

            _context.RoomTypes.Remove( entity );
            _context.SaveChanges();
        }
    }
}