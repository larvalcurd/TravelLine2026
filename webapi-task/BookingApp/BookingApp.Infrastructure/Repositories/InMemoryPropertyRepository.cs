using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookingApp.Domain.Entities;
using BookingApp.Domain.Interfaces.Repositories;

namespace BookingApp.Infrastructure.Repositories
{
    public class InMemoryPropertyRepository : IPropertyRepository
    {
        private static readonly List<Property> _properties = new List<Property>();

        public Property? GetById(Guid id)
        {
            return _properties.FirstOrDefault(p => p.Id == id);
        }

        public IEnumerable<Property> GetAll()
        {
            return _properties;
        }

        public void Add(Property property)
        {
            _properties.Add(property);
        }

        public void Update(Property property)
        {
            Property? existingProperty = GetById(property.Id);

            if (existingProperty == null)
            {
                return;
            }

            existingProperty.Name = property.Name;
            existingProperty.Country = property.Country;
            existingProperty.City = property.City;
            existingProperty.Address = property.Address;
            existingProperty.Latitude = property.Latitude;
            existingProperty.Longitude = property.Longitude;
        }

        public void Delete(Guid id)
        {
            Property? property = GetById(id);
            if (property != null)
            {
                _properties.Remove(property);
            }
        }
    }
}