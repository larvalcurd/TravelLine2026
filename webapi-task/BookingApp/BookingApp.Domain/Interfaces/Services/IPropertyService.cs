using BookingApp.Domain.Entities;

namespace BookingApp.Domain.Interfaces.Services
{
    public interface IPropertyService
    {
        IReadOnlyCollection<Property> GetAll();
        Property GetById(Guid id);
        Property Create(Property property);
        Property Update(Guid id, Property property);
        void Delete(Guid Id);
    }
}