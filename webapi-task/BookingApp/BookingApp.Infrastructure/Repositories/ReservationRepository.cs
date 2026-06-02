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

        public IReadOnlyCollection<Reservation> GetAll()
        {
            return Reservations
                .AsNoTracking()
                .ToList();
        }

        public Reservation? GetById( Guid id )
        {
            return Reservations.FirstOrDefault( r => r.Id == id );
        }

        public IReadOnlyCollection<Reservation> GetOverlappingReservations(
            IEnumerable<Guid> roomTypeIds,
            DateOnly arrival,
            DateOnly departure )
        {
            var idSet = roomTypeIds.ToHashSet();

            return Reservations
                .AsNoTracking()
                .Where( r => !r.IsCanceled )
                .Where( r => idSet.Contains( r.RoomTypeId ) )
                .Where( r => arrival < r.DepartureDate && departure > r.ArrivalDate )
                .ToList();
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

        public IReadOnlyCollection<Reservation> GetByFilter( ReservationFilter filter )
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

            return query
                .OrderBy( r => r.ArrivalDate )
                .ThenBy( r => r.ArrivalTime )
                .ToList();
        }

        public bool HasReservationsForProperty( Guid propertyId )
        {
            return Reservations.Any( r => r.PropertyId == propertyId && !r.IsCanceled );
        }

        public bool HasReservationsForRoomType( Guid id )
        {
            return Reservations.Any( r => r.RoomTypeId == id && !r.IsCanceled );
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