using BookingApp.Domain.Entities;
using BookingApp.Domain.Interfaces.Repositories;
using BookingApp.Domain.Models;
using BookingApp.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BookingApp.Infrastructure.Repositories
{
    public class EfReservationRepository(BookingDbContext context) : IReservationRepository
    {
        private readonly BookingDbContext _context = context;

        public IReadOnlyCollection<Reservation> GetAll()
        {
            return _context.Reservations
                .AsNoTracking()
                .ToList();
        }

        public Reservation? GetById(Guid id)
        {
            return _context.Reservations
                .AsNoTracking()
                .FirstOrDefault(r => r.Id == id);
        }

        public IReadOnlyCollection<Reservation> GetOverlappingReservations(
            IEnumerable<Guid> roomTypeIds,
            DateOnly arrival,
            DateOnly departure)
        {
            var idSet = roomTypeIds.ToHashSet();

            return _context.Reservations
                .AsNoTracking()
                .Where(r => !r.IsCanceled)
                .Where(r => idSet.Contains(r.RoomTypeId))
                .Where(r => arrival < r.DepartureDate && departure > r.ArrivalDate)
                .ToList();
        }

        public int GetOverlappingCount(
            Guid roomTypeId,
            DateOnly arrival,
            DateOnly departure)
        {
            return _context.Reservations
                .Count(r =>
                        !r.IsCanceled &&
                        r.RoomTypeId == roomTypeId &&
                        arrival < r.DepartureDate &&
                        departure > r.ArrivalDate);
        }

        public IReadOnlyCollection<Reservation> GetByFilter(ReservationFilter filter)
        {
            IQueryable<Reservation> query = _context.Reservations.AsNoTracking();

            if (!filter.IncludeCanceled)
            {
                query = query.Where(r => !r.IsCanceled);
            }

            if (filter.PropertyId.HasValue)
            {
                query = query.Where(r => r.PropertyId == filter.PropertyId.Value);
            }

            if (filter.RoomTypeId.HasValue)
            {
                query = query.Where(r => r.RoomTypeId == filter.RoomTypeId.Value);
            }

            if (filter.ArrivalDateFrom.HasValue)
            {
                query = query.Where(r => r.ArrivalDate >= filter.ArrivalDateFrom.Value);
            }

            if (filter.ArrivalDateTo.HasValue)
            {
                query = query.Where(r => r.ArrivalDate <= filter.ArrivalDateTo.Value);
            }

            if (!string.IsNullOrWhiteSpace(filter.GuestName))
            {
                var name = filter.GuestName.ToLower();
                query = query.Where(r => r.GuestName.ToLower().Contains(name));
            }

            if (!string.IsNullOrWhiteSpace(filter.GuestPhoneNumber))
            {
                query = query.Where(r =>
                    r.GuestPhoneNumber.Contains(filter.GuestPhoneNumber));
            }

            return query
                .OrderBy(r => r.ArrivalDate)
                .ThenBy(r => r.ArrivalTime)
                .ToList();
        }

        public bool HasReservationsForProperty(Guid propertyId)
        {
            return _context.Reservations.Any(r => r.PropertyId == propertyId && !r.IsCanceled);
        }

        public bool HasReservationsForRoomType(Guid id)
        {
            return _context.Reservations.Any(r => r.RoomTypeId == id && !r.IsCanceled);
        }

        public void Add(Reservation reservation)
        {
            _context.Reservations.Add(reservation);
            _context.SaveChanges();
        }

        public void Update(Reservation reservation)
        {
            var existing = _context.Reservations.Find(reservation.Id)
                ?? throw new InvalidOperationException($"Reservation with id '{reservation.Id}' was not found.");

            _context.Entry(existing).CurrentValues.SetValues(reservation);
            _context.SaveChanges();
        }
    }
}