
using BookingApp.Domain.Entities;

namespace BookingApp.Domain.Interfaces.Repositories
{
    public interface IPropertyRepository
    {
        Property? GetById(Guid id);
        IEnumerable<Property> GetAll();
        void Add(Property property);
        void Update(Property property);
        void Delete(Guid id);
    }
}