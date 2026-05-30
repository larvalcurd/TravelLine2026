using BookingApp.Domain.Entities;
using BookingApp.Domain.Interfaces.Repositories;
using BookingApp.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BookingApp.Infrastructure.Repositories
{
    public class EfPropertyRepository(BookingDbContext context) : IPropertyRepository
    {
        private readonly BookingDbContext _context = context;

        public IReadOnlyCollection<Property> GetAll()
        {
            return _context.Properties.AsNoTracking().Include(p => p.RoomTypes).ToList();
        }

        public Property? GetById(Guid id)
        {
            return _context.Properties.AsNoTracking().Include(p => p.RoomTypes).FirstOrDefault(p => p.Id == id);
        }

        public IReadOnlyCollection<Property> GetByCity(string city)
        {
            return _context.Properties.AsNoTracking().Where(p => p.City.ToLower() == city.ToLower()).ToList();
        }

        public void Add(Property property)
        {
            _context.Properties.Add(property);
            _context.SaveChanges();
        }

        public void Update(Property property)
        {
            var existing = _context.Properties.Find(property.Id)
                ?? throw new InvalidOperationException($"Property with id '{property.Id}' was not found.");

            _context.Entry(existing).CurrentValues.SetValues(property);
            _context.SaveChanges();
        }

        public void Delete(Guid id)
        {
            var entity = _context.Properties.Find(id)
                ?? throw new InvalidOperationException($"Property with id '{id}' was not found.");

            _context.Properties.Remove(entity);
            _context.SaveChanges();
        }
    }
}
