using BookingApp.Domain.Entities;

namespace BookingApp.Domain.Interfaces.Repositories
{
    public interface IRoomTypeRepository
    {
        IReadOnlyCollection<RoomType> GetAll();
        IReadOnlyCollection<RoomType> GetByPropertyId(Guid propertyId);
        IReadOnlyCollection<RoomType> GetByPropertyIds(IEnumerable<Guid> propertyIds);
        IReadOnlyCollection<RoomType> GetCandidates(IEnumerable<Guid> propertyIds, int guests, decimal? maxPrice);
        bool HasRoomTypesForProperty(Guid propertyId);
        RoomType? GetById(Guid id);
        void Add(RoomType roomType);
        void Update(RoomType roomType);
        void Delete(Guid id);
    }
}