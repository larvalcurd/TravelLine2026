using BookingApp.Domain.Entities;
using BookingApp.Domain.Interfaces.Repositories;

namespace BookingApp.Infrastructure.Repositories
{
    public class InMemoryRoomTypeRepository : IRoomTypeRepository
    {
        private static readonly List<RoomType> _roomTypes = new List<RoomType>();

        public RoomType? GetById(Guid id)
        {
            return _roomTypes.FirstOrDefault(r => r.Id == id);
        }

        public IEnumerable<RoomType> GetAll()
        {
            return _roomTypes;
        }

        public IEnumerable<RoomType> GetByPropertyId(Guid propertyId)
        {
            return _roomTypes.Where(r => r.PropertyId == propertyId);
        }

        public void Add(RoomType roomType)
        {
            _roomTypes.Add(roomType);
        }

        public void Update(RoomType roomType)
        {
            RoomType? existingRoomType = GetById(roomType.Id);

            if (existingRoomType == null)
            {
                return;
            }

            existingRoomType.Name = roomType.Name;
            existingRoomType.DailyPrice = roomType.DailyPrice;
            existingRoomType.Currency = roomType.Currency;

            existingRoomType.TotalRoomsCount = roomType.TotalRoomsCount;

            existingRoomType.MinPersonCount = roomType.MinPersonCount;
            existingRoomType.MaxPersonCount = roomType.MaxPersonCount;

            existingRoomType.Services = roomType.Services;
            existingRoomType.Amenities = roomType.Amenities;
        }

        public void Delete(Guid id)
        {
            RoomType? roomType = GetById(id);
            if (roomType != null)
            {
                _roomTypes.Remove(roomType);
            }
        }
    }
}