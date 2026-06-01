using BookingApp.Domain.Entities;
using BookingApp.Domain.Exceptions;
using BookingApp.Domain.Interfaces.Repositories;
using BookingApp.Domain.Interfaces.Services;

namespace BookingApp.Domain.Services
{
    public class PropertyService( IPropertyRepository propertyRepository, IRoomTypeRepository roomTypeRepository, IReservationRepository reservationRepository ) : IPropertyService
    {
        private readonly IPropertyRepository _propertyRepository = propertyRepository;
        private readonly IRoomTypeRepository _roomTypeRepository = roomTypeRepository;
        private readonly IReservationRepository _reservationRepository = reservationRepository;

        public IReadOnlyCollection<Property> GetAll()
        {

            var properties = _propertyRepository.GetAll().ToList();

            if ( properties.Count == 0 )
            {
                return [];
            }

            var propertyIds = properties.Select( p => p.Id ).ToList();

            var allRoomTypes = _roomTypeRepository.GetByPropertyIds( propertyIds ).GroupBy( rt => rt.PropertyId ).ToDictionary( g => g.Key, g => g.ToList() );

            foreach ( var property in properties )
            {
                if ( allRoomTypes.TryGetValue( property.Id, out var roomTypes ) )
                {
                    property.RoomTypes = roomTypes;
                }
                else
                {
                    property.RoomTypes = [];
                }
            }

            return properties;
        }

        public Property GetById( Guid id )
        {
            Property? property = _propertyRepository.GetById( id ) ?? throw new NotFoundException( $"Property with id '{id}' was not found." );
            return AttachRoomTypes( property );
        }

        public Property Create( Property property )
        {
            Validate( property );

            var entity = new Property
            {
                Id = Guid.NewGuid(),
                Name = property.Name.Trim(),
                Country = property.Country.Trim(),
                City = property.City.Trim(),
                Address = property.Address.Trim(),
                Latitude = property.Latitude,
                Longitude = property.Longitude
            };

            _propertyRepository.Add( entity );
            return AttachRoomTypes( entity );
        }

        public Property Update( Guid id, Property property )
        {
            Property existing = _propertyRepository.GetById( id ) ?? throw new NotFoundException( $"Property with id '{id}' was not found." );

            Validate( property );

            var updated = new Property
            {
                Id = existing.Id,
                Name = property.Name.Trim(),
                Country = property.Country.Trim(),
                City = property.City.Trim(),
                Address = property.Address.Trim(),
                Latitude = property.Latitude,
                Longitude = property.Longitude
            };

            _propertyRepository.Update( updated );
            return AttachRoomTypes( updated );
        }

        public void Delete( Guid id )
        {
            var existing = _propertyRepository.GetById( id ) ?? throw new NotFoundException( $"Property with id '{id}' was not found." );

            if ( _roomTypeRepository.HasRoomTypesForProperty( id ) )
            {
                throw new ValidationException( "Cannot delete property with existing room types." );
            }

            if ( _reservationRepository.HasReservationsForProperty( id ) )
            {
                throw new ValidationException( "Cannot delete property with existing reservations." );
            }

            _propertyRepository.Delete( id );
        }

        private static void Validate( Property property )
        {
            if ( property == null )
            {
                throw new ValidationException( "Property is required." );
            }

            if ( string.IsNullOrWhiteSpace( property.Name ) )
            {
                throw new ValidationException( "Property name is required." );
            }

            if ( string.IsNullOrWhiteSpace( property.Country ) )
            {
                throw new ValidationException( "Country is required." );
            }

            if ( string.IsNullOrWhiteSpace( property.City ) )
            {
                throw new ValidationException( "City is required." );
            }

            if ( string.IsNullOrWhiteSpace( property.Address ) )
            {
                throw new ValidationException( "Address is required." );
            }

            if ( property.Latitude < -90 || property.Latitude > 90 )
            {
                throw new ValidationException( "Latitude must be between -90 and 90." );
            }

            if ( property.Longitude < -180 || property.Longitude > 180 )
            {
                throw new ValidationException( "Longitude must be between -180 and 180." );
            }
        }

        private Property AttachRoomTypes( Property property )
        {
            property.RoomTypes = _roomTypeRepository.GetByPropertyId( property.Id ).ToList();
            return property;
        }
    }
}
