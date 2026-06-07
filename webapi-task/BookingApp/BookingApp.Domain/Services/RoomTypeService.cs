using BookingApp.Domain.Entities;
using BookingApp.Domain.Exceptions;
using BookingApp.Domain.Interfaces.Repositories;
using BookingApp.Domain.Interfaces.Services;
using BookingApp.Domain.Models;

namespace BookingApp.Domain.Services
{
    public class RoomTypeService(
        IPropertyRepository propertyRepository,
        IRoomTypeRepository roomTypeRepository,
        IReservationRepository reservationRepository ) : IRoomTypeService
    {
        private readonly IPropertyRepository _propertyRepository = propertyRepository;
        private readonly IRoomTypeRepository _roomTypeRepository = roomTypeRepository;
        private readonly IReservationRepository _reservationRepository = reservationRepository;

        public async Task<IReadOnlyCollection<RoomType>> GetByPropertyIdAsync( Guid propertyId )
        {
            await EnsurePropertyExistsAsync( propertyId );
            return await _roomTypeRepository.GetByPropertyIdAsync( propertyId );
        }

        public async Task<RoomType> GetByIdAsync( Guid id )
        {
            return await _roomTypeRepository.GetByIdAsync( id )
                ?? throw new NotFoundException( $"Room type with id '{id}' was not found." );
        }

        public RoomType Create( Guid propertyId, CreateRoomTypeRequest request )
        {
            EnsurePropertyExists( propertyId );
            Validate( request );

            RoomType entity = new()
            {
                Id = Guid.NewGuid(),
                PropertyId = propertyId,
                Name = request.Name.Trim(),
                DailyPrice = request.DailyPrice,
                Currency = request.Currency.Trim(),
                MinPersonCount = request.MinPersonCount,
                MaxPersonCount = request.MaxPersonCount,
                TotalRoomsCount = request.TotalRoomsCount,
                Services = NormalizeList( request.Services ),
                Amenities = NormalizeList( request.Amenities )
            };

            _roomTypeRepository.Add( entity );
            return entity;
        }

        public RoomType Update( Guid id, UpdateRoomTypeRequest request )
        {
            RoomType? existing = _roomTypeRepository.GetById( id )
                ?? throw new NotFoundException( $"Room type with id '{id}' was not found." );

            Validate( request );

            existing.Update(
                request.Name.Trim(),
                request.DailyPrice,
                request.Currency.Trim(),
                request.MinPersonCount,
                request.MaxPersonCount,
                request.TotalRoomsCount,
                NormalizeList( request.Services ),
                NormalizeList( request.Amenities ) );

            _roomTypeRepository.Update( existing );
            return existing;
        }

        public void Delete( Guid id )
        {
            RoomType? existing = _roomTypeRepository.GetById( id )
                ?? throw new NotFoundException( $"Room type with id '{id}' was not found." );

            if ( _reservationRepository.HasReservationsForRoomType( id ) )
            {
                throw new ValidationException( "Cannot delete room type with existing reservations." );
            }


            _roomTypeRepository.Delete( id );

        }

        private void EnsurePropertyExists( Guid propertyId )
        {
            if ( _propertyRepository.GetById( propertyId ) == null )
            {
                throw new NotFoundException( $"Property with id '{propertyId}' was not found." );
            }
        }

        private async Task EnsurePropertyExistsAsync( Guid propertyId )
        {
            if ( await _propertyRepository.GetByIdAsync( propertyId ) == null )
            {
                throw new NotFoundException( $"Property with id '{propertyId}' was not found." );
            }
        }

        private static void Validate( CreateRoomTypeRequest request )
        {
            ArgumentNullException.ThrowIfNull( request );
            ValidateCore(
                request.Name,
                request.DailyPrice,
                request.Currency,
                request.MinPersonCount,
                request.MaxPersonCount,
                request.TotalRoomsCount );
        }

        private static void Validate( UpdateRoomTypeRequest request )
        {
            ArgumentNullException.ThrowIfNull( request );
            ValidateCore(
                request.Name,
                request.DailyPrice,
                request.Currency,
                request.MinPersonCount,
                request.MaxPersonCount,
                request.TotalRoomsCount );
        }

        private static void ValidateCore(
            string name,
            decimal dailyPrice,
            string currency,
            int minPersonCount,
            int maxPersonCount,
            int totalRoomsCount )
        {
            if ( string.IsNullOrWhiteSpace( name ) )
            {
                throw new ValidationException( "Room type name is required." );
            }

            if ( dailyPrice <= 0 )
            {
                throw new ValidationException( "Daily price must be greater than zero." );
            }

            if ( string.IsNullOrWhiteSpace( currency ) )
            {
                throw new ValidationException( "Currency is required." );
            }

            if ( minPersonCount <= 0 )
            {
                throw new ValidationException( "MinPersonCount must be greater than zero." );
            }

            if ( maxPersonCount < minPersonCount )
            {
                throw new ValidationException( "MaxPersonCount must be greater than or equal to MinPersonCount." );
            }

            if ( totalRoomsCount <= 0 )
            {
                throw new ValidationException( "TotalRoomsCount must be greater than zero." );
            }
        }

        private static List<string> NormalizeList( IEnumerable<string>? values )
        {
            return values?
                .Select( x => x?.Trim() )
                .Where( x => !string.IsNullOrWhiteSpace( x ) )
                .Cast<string>()
                .ToList()
                ?? [];
        }
    }
}
