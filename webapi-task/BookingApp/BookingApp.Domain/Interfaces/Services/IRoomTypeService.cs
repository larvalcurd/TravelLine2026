using BookingApp.Domain.Entities;

namespace BookingApp.Domain.Interfaces.Services
{
    public interface IRoomTypeService
    {
        IReadOnlyCollection<RoomType> GetByPropertyId( Guid propertyId );
        RoomType GetById( Guid id );
        RoomType Create( Guid propertyId, RoomType roomType );
        RoomType Update( Guid id, RoomType roomType );
        void Delete( Guid id );
    }
}