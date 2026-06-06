using BookingApp.Domain.Entities;
using BookingApp.Domain.Models;

namespace BookingApp.Domain.Interfaces.Services
{
    public interface IReservationService
    {
        Reservation Create( CreateReservationRequest request );
        void Cancel( Guid id );

        Task<IReadOnlyCollection<Reservation>> GetAllAsync( ReservationFilter? filter );
        Task<Reservation> GetByIdAsync( Guid id );
    }
}