using BookingApp.Domain.Entities;
using BookingApp.Domain.Models;

namespace BookingApp.Domain.Interfaces.Repositories
{
    public interface IReservationRepository
    {
        IReadOnlyCollection<Reservation> GetAll();
        Reservation? GetById(Guid id);
        IReadOnlyCollection<Reservation> GetOverlappingReservations(IEnumerable<Guid> roomTypeIds, DateOnly arrival, DateOnly departure);
        int GetOverlappingCount(Guid roomTypeId, DateOnly arrival, DateOnly departure);
        IReadOnlyCollection<Reservation> GetByFilter(ReservationFilter filter);
        bool HasReservationsForProperty(Guid propertyId);
        bool HasReservationsForRoomType(Guid id);
        void Add(Reservation reservation);
        void Update(Reservation reservation);
    }
}