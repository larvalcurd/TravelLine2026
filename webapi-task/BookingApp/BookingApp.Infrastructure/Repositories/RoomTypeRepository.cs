using BookingApp.Domain.Entities;
using BookingApp.Domain.Interfaces.Repositories;
using BookingApp.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BookingApp.Infrastructure.Repositories
{
    public class RoomTypeRepository( BookingDbContext context ) : IRoomTypeRepository
    {
        private DbSet<RoomType> RoomTypes => context.RoomTypes;

        public async Task<IReadOnlyCollection<RoomType>> GetAllAsync()
        {
            return await RoomTypes.AsNoTracking().ToListAsync();
        }

        public async Task<IReadOnlyCollection<RoomType>> GetByPropertyIdAsync( Guid propertyId )
        {
            return await RoomTypes.AsNoTracking()
                .Where( rt => rt.PropertyId == propertyId )
                .ToListAsync();
        }

        public async Task<IReadOnlyCollection<RoomType>> GetByPropertyIdsAsync( IEnumerable<Guid> propertyIds )
        {
            var idSet = propertyIds.ToHashSet();

            return await RoomTypes.AsNoTracking()
                .Where( rt => idSet.Contains( rt.PropertyId ) )
                .ToListAsync();
        }

        public async Task<IReadOnlyCollection<RoomType>> GetCandidatesAsync(
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

            return await query.ToListAsync();
        }

        public async Task<bool> HasRoomTypesForPropertyAsync( Guid propertyId )
        {
            return await RoomTypes.AnyAsync( rt => rt.PropertyId == propertyId );
        }

        public async Task<RoomType?> GetByIdAsync( Guid id )
        {
            return await RoomTypes.FirstOrDefaultAsync( rt => rt.Id == id );
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