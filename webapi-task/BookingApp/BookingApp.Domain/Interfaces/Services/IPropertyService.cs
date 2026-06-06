using BookingApp.Domain.Entities;
using BookingApp.Domain.Models;

namespace BookingApp.Domain.Interfaces.Services
{
    public interface IPropertyService
    {
        Task<IReadOnlyCollection<Property>> GetAllAsync();
        Task<Property> GetByIdAsync( Guid id );

        Property Create( CreatePropertyRequest request );
        Property Update( Guid id, UpdatePropertyRequest request );
        void Delete( Guid id );
    }
}