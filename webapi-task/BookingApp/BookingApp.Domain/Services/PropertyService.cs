using BookingApp.Domain.Entities;
using BookingApp.Domain.Exceptions;
using BookingApp.Domain.Interfaces.Repositories;
using BookingApp.Domain.Interfaces.Services;

namespace BookingApp.Domain.Services
{
    public class PropertyService( IPropertyRepository propertyRepository, IReservationRepository reservationRepository ) : IPropertyService
    {
        private readonly IPropertyRepository _propertyRepository = propertyRepository;
        private readonly IReservationRepository _reservationRepository = reservationRepository;

        public IReadOnlyCollection<Property> GetAll()
        {
            return _propertyRepository.GetAll();
        }

        public Property GetById( Guid id )
        {
            return _propertyRepository.GetById( id )
                ?? throw new NotFoundException( $"Property with id '{id}' was not found." );
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
            return entity;
        }

        public Property Update( Guid id, Property property )
        {
            Property existing = _propertyRepository.GetById( id )
                ?? throw new NotFoundException( $"Property with id '{id}' was not found." );

            Validate( property );

            existing.Name = property.Name.Trim();
            existing.Country = property.Country.Trim();
            existing.City = property.City.Trim();
            existing.Address = property.Address.Trim();
            existing.Latitude = property.Latitude;
            existing.Longitude = property.Longitude;

            _propertyRepository.Update( existing );

            return existing;
        }

        public void Delete( Guid id )
        {
            var existing = _propertyRepository.GetById( id )
                ?? throw new NotFoundException( $"Property with id '{id}' was not found." );

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
    }
}
