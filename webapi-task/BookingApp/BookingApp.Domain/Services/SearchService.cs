using BookingApp.Domain.Entities;
using BookingApp.Domain.Interfaces.Repositories;
using BookingApp.Domain.Interfaces.Services;
using BookingApp.Domain.Models;

namespace BookingApp.Domain.Services;

public class SearchService(
    IPropertyRepository propertyRepository,
    IRoomTypeRepository roomTypeRepository,
    IReservationRepository reservationRepository ) : ISearchService
{
    private readonly IPropertyRepository _propertyRepository = propertyRepository;
    private readonly IRoomTypeRepository _roomTypeRepository = roomTypeRepository;
    private readonly IReservationRepository _reservationRepository = reservationRepository;

    public IReadOnlyCollection<AvailableRoomOption> Search( SearchAvailabilityCriteria criteria )
    {
        if ( !IsValidCriteria( criteria ) )
        {
            return [];
        }

        var propertiesInCity = _propertyRepository.GetByCity( criteria.City ).ToDictionary( p => p.Id, p => p );
        if ( propertiesInCity.Count == 0 )
        {
            return [];
        }

        var candidateRoomTypes = _roomTypeRepository
            .GetCandidates( propertiesInCity.Keys, criteria.Guests, criteria.MaxPrice )
            .ToList();

        if ( candidateRoomTypes.Count == 0 )
        {
            return [];
        }

        var overlappingCounts = GetOverlappingCounts( candidateRoomTypes.Select( rt => rt.Id ), criteria );
        int nights = criteria.DepartureDate.DayNumber - criteria.ArrivalDate.DayNumber;

        return BuildAvailableOptions( candidateRoomTypes, propertiesInCity, overlappingCounts, nights );
    }

    private static bool IsValidCriteria( SearchAvailabilityCriteria criteria )
    {
        return criteria != null
            && !string.IsNullOrWhiteSpace( criteria.City )
            && criteria.Guests > 0
            && criteria.ArrivalDate < criteria.DepartureDate;
    }

    private Dictionary<Guid, int> GetOverlappingCounts( IEnumerable<Guid> roomTypeIds, SearchAvailabilityCriteria criteria )
    {
        return _reservationRepository
            .GetOverlappingReservations( roomTypeIds, criteria.ArrivalDate, criteria.DepartureDate )
            .GroupBy( r => r.RoomTypeId )
            .ToDictionary( g => g.Key, g => g.Count() );
    }

    private static List<AvailableRoomOption> BuildAvailableOptions(
        List<RoomType> candidateRoomTypes,
        Dictionary<Guid, Property> propertiesInCity,
        Dictionary<Guid, int> overlappingCounts,
        int nights )
    {
        var result = new List<AvailableRoomOption>();

        foreach ( var roomType in candidateRoomTypes )
        {
            var bookedCount = overlappingCounts.GetValueOrDefault( roomType.Id, 0 );
            var availableCount = roomType.TotalRoomsCount - bookedCount;

            if ( availableCount > 0 )
            {
                var property = propertiesInCity[ roomType.PropertyId ];
                result.Add( MapToOption( property, roomType, availableCount, nights ) );
            }
        }

        return result
            .OrderBy( x => x.DailyPrice )
            .ThenBy( x => x.PropertyName )
            .ThenBy( x => x.RoomTypeName )
            .ToList();
    }

    private static AvailableRoomOption MapToOption( Property property, RoomType roomType, int availableCount, int nights )
    {
        return new AvailableRoomOption
        {
            PropertyId = property.Id,
            PropertyName = property.Name,
            Country = property.Country,
            City = property.City,
            Address = property.Address,
            Latitude = property.Latitude,
            Longitude = property.Longitude,
            RoomTypeId = roomType.Id,
            RoomTypeName = roomType.Name,
            DailyPrice = roomType.DailyPrice,
            Currency = roomType.Currency,
            MinPersonCount = roomType.MinPersonCount,
            MaxPersonCount = roomType.MaxPersonCount,
            AvailableRoomsCount = availableCount,
            Nights = nights,
            TotalPrice = nights * roomType.DailyPrice,
            Services = roomType.Services is null ? [] : [ .. roomType.Services ],
            Amenities = roomType.Amenities is null ? [] : [ .. roomType.Amenities ]
        };
    }
}