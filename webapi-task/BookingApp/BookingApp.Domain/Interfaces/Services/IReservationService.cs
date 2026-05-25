using BookingApp.Domain.Entities;
using BookingApp.Domain.Models;

namespace BookingApp.Domain.Interfaces.Services
{
    public interface IReservationService
    {
        Reservation Create(CreateReservationRequest request);
        IReadOnlyCollection<Reservation> GetAll(ReservationFilter? filter);
        Reservation GetById(Guid id);
        void Cancel(Guid id);
    }
}