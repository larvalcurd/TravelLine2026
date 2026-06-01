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
        if ( criteria == null || string.IsNullOrWhiteSpace( criteria.City ) ||
            criteria.Guests <= 0 || criteria.ArrivalDate >= criteria.DepartureDate )
        {
            return [];
        }

        var nights = criteria.DepartureDate.DayNumber - criteria.ArrivalDate.DayNumber;

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

        var candidateRoomTypeIds = candidateRoomTypes.Select( rt => rt.Id ).ToHashSet();

        var overlappingCounts = _reservationRepository
            .GetOverlappingReservations( candidateRoomTypeIds, criteria.ArrivalDate, criteria.DepartureDate )
            .GroupBy( r => r.RoomTypeId )
            .ToDictionary( g => g.Key, g => g.Count() );

        var result = new List<AvailableRoomOption>();

        foreach ( var roomType in candidateRoomTypes )
        {
            var bookedCount = overlappingCounts.TryGetValue( roomType.Id, out var count )
                ? count
                : 0;

            var availableCount = roomType.TotalRoomsCount - bookedCount;
            if ( availableCount <= 0 )
            {
                continue;
            }

            var property = propertiesInCity[ roomType.PropertyId ];

            result.Add( new AvailableRoomOption
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
            } );
        }

        return result
            .OrderBy( x => x.DailyPrice )
            .ThenBy( x => x.PropertyName )
            .ThenBy( x => x.RoomTypeName )
            .ToList();
    }
}