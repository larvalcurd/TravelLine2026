
using BookingApp.Domain.Entities;

namespace BookingApp.Domain.Interfaces.Repositories
{
    public interface IPropertyRepository
    {
        IReadOnlyCollection<Property> GetAll();
        Property? GetById(Guid id);
        IReadOnlyCollection<Property> GetByCity(string city);
        void Add(Property property);
        void Update(Property property);
        void Delete(Guid id);
    }
}