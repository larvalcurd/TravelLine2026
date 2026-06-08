using BookingApp.Domain.Models;

namespace BookingApp.Domain.Interfaces.Services
{
    public interface ISearchService
    {
        Task<IReadOnlyCollection<AvailableRoomOption>> SearchAsync( SearchAvailabilityCriteria criteria );
    }
}