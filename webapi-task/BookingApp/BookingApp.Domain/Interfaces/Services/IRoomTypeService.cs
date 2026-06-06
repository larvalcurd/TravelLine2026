using BookingApp.Domain.Entities;
using BookingApp.Domain.Models;

namespace BookingApp.Domain.Interfaces.Services
{
    public interface IRoomTypeService
    {
        Task<IReadOnlyCollection<RoomType>> GetByPropertyIdAsync( Guid propertyId );
        Task<RoomType> GetByIdAsync( Guid id );

        RoomType Create( Guid propertyId, CreateRoomTypeRequest request );
        RoomType Update( Guid id, UpdateRoomTypeRequest request );
        void Delete( Guid id );
    }
}