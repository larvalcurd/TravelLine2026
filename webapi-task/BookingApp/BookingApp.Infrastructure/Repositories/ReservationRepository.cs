using BookingApp.Domain.Entities;
using BookingApp.Domain.Interfaces.Repositories;
using BookingApp.Domain.Models;
using BookingApp.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BookingApp.Infrastructure.Repositories
{
    public class ReservationRepository( BookingDbContext context ) : IReservationRepository
    {
        private DbSet<Reservation> Reservations => context.Reservations;

        public async Task<IReadOnlyCollection<Reservation>> GetAllAsync()
        {
            return await Reservations
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Reservation?> GetByIdAsync( Guid id )
        {
            return await Reservations.FirstOrDefaultAsync( r => r.Id == id );
        }

        public async Task<IReadOnlyCollection<Reservation>> GetOverlappingReservationsAsync(
            IEnumerable<Guid> roomTypeIds,
            DateOnly arrival,
            DateOnly departure )
        {
            var idSet = roomTypeIds.ToHashSet();

            return await Reservations
                .AsNoTracking()
                .Where( r => !r.IsCanceled )
                .Where( r => idSet.Contains( r.RoomTypeId ) )
                .Where( r => arrival < r.DepartureDate && departure > r.ArrivalDate )
                .ToListAsync();
        }

        public async Task<int> GetOverlappingCountAsync(
            Guid roomTypeId,
            DateOnly arrival,
            DateOnly departure )
        {
            return await Reservations
                .CountAsync( r =>
                        !r.IsCanceled &&
                        r.RoomTypeId == roomTypeId &&
                        arrival < r.DepartureDate &&
                        departure > r.ArrivalDate );
        }

        public async Task<IReadOnlyCollection<Reservation>> GetByFilterAsync( ReservationFilter filter )
        {
            IQueryable<Reservation> query = Reservations.AsNoTracking();

            if ( !filter.IncludeCanceled )
            {
                query = query.Where( r => !r.IsCanceled );
            }

            if ( filter.PropertyId.HasValue )
            {
                query = query.Where( r => r.PropertyId == filter.PropertyId.Value );
            }

            if ( filter.RoomTypeId.HasValue )
            {
                query = query.Where( r => r.RoomTypeId == filter.RoomTypeId.Value );
            }

            if ( filter.ArrivalDateFrom.HasValue )
            {
                query = query.Where( r => r.ArrivalDate >= filter.ArrivalDateFrom.Value );
            }

            if ( filter.ArrivalDateTo.HasValue )
            {
                query = query.Where( r => r.ArrivalDate <= filter.ArrivalDateTo.Value );
            }

            if ( !string.IsNullOrWhiteSpace( filter.GuestName ) )
            {
                var name = filter.GuestName.ToLower();
                query = query.Where( r => r.GuestName.ToLower().Contains( name ) );
            }

            if ( !string.IsNullOrWhiteSpace( filter.GuestPhoneNumber ) )
            {
                query = query.Where( r =>
                    r.GuestPhoneNumber.Contains( filter.GuestPhoneNumber ) );
            }

            return await query
                .OrderBy( r => r.ArrivalDate )
                .ThenBy( r => r.ArrivalTime )
                .ToListAsync();
        }

        public async Task<bool> HasReservationsForPropertyAsync( Guid propertyId )
        {
            return await Reservations.AnyAsync( r => r.PropertyId == propertyId && !r.IsCanceled );
        }

        public async Task<bool> HasReservationsForRoomTypeAsync( Guid id )
        {
            return await Reservations.AnyAsync( r => r.RoomTypeId == id && !r.IsCanceled );
        }

        public Reservation? GetById( Guid id )
        {
            return Reservations.FirstOrDefault( r => r.Id == id );
        }

        public bool HasReservationsForProperty( Guid propertyId )
        {
            return Reservations.Any( r => r.PropertyId == propertyId && !r.IsCanceled );
        }

        public bool HasReservationsForRoomType( Guid id )
        {
            return Reservations.Any( r => r.RoomTypeId == id && !r.IsCanceled );
        }

        public int GetOverlappingCount(
            Guid roomTypeId,
            DateOnly arrival,
            DateOnly departure )
        {
            return Reservations
                .Count( r =>
                    !r.IsCanceled &&
                    r.RoomTypeId == roomTypeId &&
                    arrival < r.DepartureDate &&
                    departure > r.ArrivalDate );
        }

        public void Add( Reservation reservation )
        {
            Reservations.Add( reservation );
            context.SaveChanges();
        }

        public void Update( Reservation reservation )
        {
            Reservations.Update( reservation );
            context.SaveChanges();
        }
    }
}