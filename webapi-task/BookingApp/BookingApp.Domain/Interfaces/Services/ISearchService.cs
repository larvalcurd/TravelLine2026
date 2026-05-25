using BookingApp.Domain.Models;

namespace BookingApp.Domain.Interfaces.Services
{
    public interface ISearchService
    {
        IReadOnlyCollection<AvailableRoomOption> Search(SearchAvailabilityCriteria criteria);
    }
}