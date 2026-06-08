using BookingApp.Domain.Entities;
using BookingApp.Domain.Models;

namespace BookingApp.Domain.Interfaces.Repositories
{
    public interface IReservationRepository
    {
        Task<IReadOnlyCollection<Reservation>> GetAllAsync();
        Task<Reservation?> GetByIdAsync( Guid id );
        Task<IReadOnlyCollection<Reservation>> GetOverlappingReservationsAsync( IEnumerable<Guid> roomTypeIds, DateOnly arrival, DateOnly departure );
        Task<int> GetOverlappingCountAsync( Guid roomTypeId, DateOnly arrival, DateOnly departure );
        Task<IReadOnlyCollection<Reservation>> GetByFilterAsync( ReservationFilter filter );
        Task<bool> HasReservationsForPropertyAsync( Guid propertyId );
        Task<bool> HasReservationsForRoomTypeAsync( Guid id );

        Reservation? GetById( Guid id );
        bool HasReservationsForProperty( Guid propertyId );
        bool HasReservationsForRoomType( Guid id );
        int GetOverlappingCount( Guid roomTypeId, DateOnly arrival, DateOnly departure );

        void Add( Reservation reservation );
        void Update( Reservation reservation );
    }
}