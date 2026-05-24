using BookingApp.Domain.Entities;

namespace BookingApp.Domain.Interfaces.Repositories
{
    public interface IReservationRepository
    {
        Reservation? GetById(Guid id);
        IEnumerable<Reservation> GetAll();
        public void Add(Reservation reservation);
        public void Update(Reservation reservation);
        public void Delete(Guid id);
    }
}