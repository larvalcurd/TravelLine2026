using BookingApp.Domain.Entities;
using BookingApp.Domain.Interfaces.Repositories;
namespace BookingApp.Infrastructure.Repositories
{
    public class InMemoryReservationRepository : IReservationRepository
    {
        private static readonly List<Reservation> _reservations = new List<Reservation>();

        public Reservation? GetById(Guid id)
        {
            return _reservations.FirstOrDefault(r => r.Id == id);
        }

        public IEnumerable<Reservation> GetAll()
        {
            return _reservations;
        }

        public void Add(Reservation reservation)
        {
            _reservations.Add(reservation);
        }

        public void Update(Reservation reservation)
        {
            var existingReservation = GetById(reservation.Id);

            if (existingReservation == null)
                return;

            existingReservation.PropertyId = reservation.PropertyId;
            existingReservation.RoomTypeId = reservation.RoomTypeId;
            existingReservation.CheckInDateTime = reservation.CheckInDateTime;
            existingReservation.CheckOutDateTime = reservation.CheckOutDateTime;
            existingReservation.GuestName = reservation.GuestName;
            existingReservation.GuestPhoneNumber = reservation.GuestPhoneNumber;
            existingReservation.Total = reservation.Total;
            existingReservation.Currency = reservation.Currency;
        }
        
        public void Delete(Guid id)
        {
            Reservation? reservation = GetById(id);
            if (reservation != null)
            {
                _reservations.Remove(reservation);
            }
        }
    }
}