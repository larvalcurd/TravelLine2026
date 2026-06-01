using BookingApp.Domain.Entities;
using BookingApp.Domain.Exceptions;
using BookingApp.Domain.Interfaces.Repositories;
using BookingApp.Domain.Interfaces.Services;

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

        public IReadOnlyCollection<RoomType> GetByPropertyId( Guid propertyId )
        {
            EnsurePropertyExists( propertyId );
            return _roomTypeRepository.GetByPropertyId( propertyId );
        }

        public RoomType GetById( Guid id )
        {
            return _roomTypeRepository.GetById( id ) ?? throw new NotFoundException( $"Room type with id '{id}' was not found." );
        }

        public RoomType Create( Guid propertyId, RoomType roomType )
        {
            Validate( roomType );
            EnsurePropertyExists( propertyId );

            var entity = new RoomType
            {
                Id = Guid.NewGuid(),
                PropertyId = propertyId,
                Name = roomType.Name.Trim(),
                DailyPrice = roomType.DailyPrice,
                Currency = roomType.Currency.Trim(),
                MinPersonCount = roomType.MinPersonCount,
                MaxPersonCount = roomType.MaxPersonCount,
                TotalRoomsCount = roomType.TotalRoomsCount,
                Services = roomType.Services?.Select( x => x.Trim() ).Where( x => !string.IsNullOrWhiteSpace( x ) ).ToList() ?? [],
                Amenities = roomType.Amenities?.Select( x => x.Trim() ).Where( x => !string.IsNullOrWhiteSpace( x ) ).ToList() ?? []
            };

            _roomTypeRepository.Add( entity );
            return entity;
        }

        public RoomType Update( Guid id, RoomType roomType )
        {
            var existing = _roomTypeRepository.GetById( id ) ?? throw new NotFoundException( $"Room type with id '{id}' was not found." );

            Validate( roomType );

            var updated = new RoomType
            {
                Id = existing.Id,
                PropertyId = existing.PropertyId,
                Name = roomType.Name.Trim(),
                DailyPrice = roomType.DailyPrice,
                Currency = roomType.Currency.Trim(),
                MinPersonCount = roomType.MinPersonCount,
                MaxPersonCount = roomType.MaxPersonCount,
                TotalRoomsCount = roomType.TotalRoomsCount,
                Services = roomType.Services?.Select( x => x.Trim() ).Where( x => !string.IsNullOrWhiteSpace( x ) ).ToList() ?? [],
                Amenities = roomType.Amenities?.Select( x => x.Trim() ).Where( x => !string.IsNullOrWhiteSpace( x ) ).ToList() ?? []
            };

            _roomTypeRepository.Update( updated );
            return updated;
        }

        public void Delete( Guid id )
        {
            var existing = _roomTypeRepository.GetById( id ) ?? throw new NotFoundException( $"Room type with id '{id}' was not found." );

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

        private static void Validate( RoomType roomType )
        {
            if ( roomType == null )
            {
                throw new ValidationException( "Room type is required." );
            }

            if ( string.IsNullOrWhiteSpace( roomType.Name ) )
            {
                throw new ValidationException( "Room type name is required." );
            }

            if ( roomType.DailyPrice <= 0 )
            {
                throw new ValidationException( "Daily price must be greater than zero." );
            }

            if ( string.IsNullOrWhiteSpace( roomType.Currency ) )
            {
                throw new ValidationException( "Currency is required." );
            }

            if ( roomType.MinPersonCount <= 0 )
            {
                throw new ValidationException( "MinPersonCount must be greater than zero." );
            }

            if ( roomType.MaxPersonCount < roomType.MinPersonCount )
            {
                throw new ValidationException( "MaxPersonCount must be greater than or equal to MinPersonCount." );
            }

            if ( roomType.TotalRoomsCount <= 0 )
            {
                throw new ValidationException( "TotalRoomsCount must be greater than zero." );
            }
        }
    }
}