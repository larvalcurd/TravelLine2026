using System.ComponentModel;
using BookingApp.Domain.Entities;

namespace BookingApp.Domain.Interfaces.Repositories
{
    public interface IRoomTypeRepository
    {
        RoomType? GetById(Guid id);
        IEnumerable<RoomType> GetAll();
        IEnumerable<RoomType> GetByPropertyId(Guid propertyId);
        void Add(RoomType roomType);
        void Delete(Guid id);
    }
}