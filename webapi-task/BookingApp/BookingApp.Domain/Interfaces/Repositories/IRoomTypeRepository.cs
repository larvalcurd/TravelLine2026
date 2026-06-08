using BookingApp.Domain.Entities;

namespace BookingApp.Domain.Interfaces.Repositories
{
    public interface IRoomTypeRepository
    {
        Task<IReadOnlyCollection<RoomType>> GetAllAsync();
        Task<IReadOnlyCollection<RoomType>> GetByPropertyIdAsync( Guid propertyId );
        Task<IReadOnlyCollection<RoomType>> GetByPropertyIdsAsync( IEnumerable<Guid> propertyIds );
        Task<IReadOnlyCollection<RoomType>> GetCandidatesAsync( IEnumerable<Guid> propertyIds, int guests, decimal? maxPrice );
        Task<bool> HasRoomTypesForPropertyAsync( Guid propertyId );
        Task<RoomType?> GetByIdAsync( Guid id );

        RoomType? GetById( Guid id );

        void Add( RoomType roomType );
        void Update( RoomType roomType );
        void Delete( Guid id );
    }
}