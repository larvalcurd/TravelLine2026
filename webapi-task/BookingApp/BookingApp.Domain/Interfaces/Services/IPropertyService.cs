using BookingApp.Domain.Entities;
using BookingApp.Domain.Models;

namespace BookingApp.Domain.Interfaces.Services
{
    public interface IPropertyService
    {
        IReadOnlyCollection<Property> GetAll();
        Property GetById( Guid id );
        Property Create( CreatePropertyRequest request );
        Property Update( Guid id, UpdatePropertyRequest request );
        void Delete( Guid Id );
    }
}