using BookingApp.Domain.Entities;
using BookingApp.Domain.Exceptions;
using BookingApp.Domain.Interfaces.Repositories;
using BookingApp.Domain.Interfaces.Services;
using BookingApp.Domain.Models;

namespace BookingApp.Domain.Services
{
    public class PropertyService( IPropertyRepository propertyRepository, IReservationRepository reservationRepository ) : IPropertyService
    {
        private readonly IPropertyRepository _propertyRepository = propertyRepository;
        private readonly IReservationRepository _reservationRepository = reservationRepository;

        public async Task<IReadOnlyCollection<Property>> GetAllAsync()
        {
            return await _propertyRepository.GetAllAsync();
        }

        public async Task<Property> GetByIdAsync( Guid id )
        {
            return await _propertyRepository.GetByIdAsync( id )
                ?? throw new NotFoundException( $"Property with id '{id}' was not found." );
        }

        public Property Create( CreatePropertyRequest request )
        {
            Validate( request );

            Property entity = new()
            {
                Id = Guid.NewGuid(),
                Name = request.Name.Trim(),
                Country = request.Country.Trim(),
                City = request.City.Trim(),
                Address = request.Address.Trim(),
                Latitude = request.Latitude,
                Longitude = request.Longitude
            };

            _propertyRepository.Add( entity );
            return entity;
        }

        public Property Update( Guid id, UpdatePropertyRequest request )
        {
            Property existing = _propertyRepository.GetById( id )
                ?? throw new NotFoundException( $"Property with id '{id}' was not found." );

            Validate( request );

            existing.Update(
                request.Name.Trim(),
                request.Country.Trim(),
                request.City.Trim(),
                request.Address.Trim(),
                request.Latitude,
                request.Longitude );

            _propertyRepository.Update( existing );

            return existing;
        }

        public void Delete( Guid id )
        {
            Property? existing = _propertyRepository.GetById( id )
                ?? throw new NotFoundException( $"Property with id '{id}' was not found." );

            if ( _reservationRepository.HasReservationsForProperty( id ) )
            {
                throw new ValidationException( "Cannot delete property with existing reservations." );
            }

            _propertyRepository.Delete( id );
        }

        private static void Validate( CreatePropertyRequest request )
        {
            if ( string.IsNullOrWhiteSpace( request.Name ) )
                throw new ValidationException( "Property name is required." );

            if ( string.IsNullOrWhiteSpace( request.Country ) )
                throw new ValidationException( "Country is required." );

            if ( string.IsNullOrWhiteSpace( request.City ) )
                throw new ValidationException( "City is required." );

            if ( string.IsNullOrWhiteSpace( request.Address ) )
                throw new ValidationException( "Address is required." );

            if ( request.Latitude < -90 || request.Latitude > 90 )
                throw new ValidationException( "Latitude must be between -90 and 90." );

            if ( request.Longitude < -180 || request.Longitude > 180 )
                throw new ValidationException( "Longitude must be between -180 and 180." );
        }

        private static void Validate( UpdatePropertyRequest request )
        {
            if ( string.IsNullOrWhiteSpace( request.Name ) )
                throw new ValidationException( "Property name is required." );

            if ( string.IsNullOrWhiteSpace( request.Country ) )
                throw new ValidationException( "Country is required." );

            if ( string.IsNullOrWhiteSpace( request.City ) )
                throw new ValidationException( "City is required." );

            if ( string.IsNullOrWhiteSpace( request.Address ) )
                throw new ValidationException( "Address is required." );

            if ( request.Latitude < -90 || request.Latitude > 90 )
                throw new ValidationException( "Latitude must be between -90 and 90." );

            if ( request.Longitude < -180 || request.Longitude > 180 )
                throw new ValidationException( "Longitude must be between -180 and 180." );
        }
    }
}

