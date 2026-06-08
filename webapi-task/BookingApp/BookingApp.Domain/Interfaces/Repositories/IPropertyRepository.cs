using BookingApp.Domain.Entities;

namespace BookingApp.Domain.Interfaces.Repositories
{
    public interface IPropertyRepository
    {
        Task<IReadOnlyCollection<Property>> GetAllAsync();
        Task<Property?> GetByIdAsync( Guid id );
        Task<IReadOnlyCollection<Property>> GetByCityAsync( string city );

        public Property? GetById( Guid id );
        void Add( Property property );
        void Update( Property property );
        void Delete( Guid id );
    }
}