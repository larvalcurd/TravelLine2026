using BookingApp.Domain.Entities;
using BookingApp.Domain.Exceptions;
using BookingApp.Domain.Interfaces.Repositories;
using BookingApp.Domain.Interfaces.Services;
using BookingApp.Domain.Models;

namespace BookingApp.Domain.Services;

public class ReservationService(
    IPropertyRepository propertyRepository,
    IRoomTypeRepository roomTypeRepository,
    IReservationRepository reservationRepository) : IReservationService
{
    private readonly IPropertyRepository _propertyRepository = propertyRepository;
    private readonly IRoomTypeRepository _roomTypeRepository = roomTypeRepository;
    private readonly IReservationRepository _reservationRepository = reservationRepository;

    public Reservation Create(CreateReservationRequest request)
    {
        ValidateRequest(request);

        var property = _propertyRepository.GetById(request.PropertyId)
            ?? throw new NotFoundException($"Property with id '{request.PropertyId}' was not found.");

        var roomType = _roomTypeRepository.GetById(request.RoomTypeId)
            ?? throw new NotFoundException($"Room type with id '{request.RoomTypeId}' was not found.");

        if (roomType.PropertyId != request.PropertyId)
        {
            throw new ValidationException("The room type does not belong to the specified property.");
        }

        if (request.GuestCount < roomType.MinPersonCount || request.GuestCount > roomType.MaxPersonCount)
        {
            throw new ValidationException("Guest count does not fit the selected room type.");
        }

        var overlappingReservations = _reservationRepository.GetOverlappingCount(
            request.RoomTypeId,
            request.ArrivalDate,
            request.DepartureDate);

        if (overlappingReservations >= roomType.TotalRoomsCount)
        {
            throw new NoAvailabilityException("No available rooms for the selected period.");
        }

        var nights = request.DepartureDate.DayNumber - request.ArrivalDate.DayNumber;

        var reservation = new Reservation
        {
            Id = Guid.NewGuid(),
            PropertyId = request.PropertyId,
            RoomTypeId = request.RoomTypeId,
            ArrivalDate = request.ArrivalDate,
            DepartureDate = request.DepartureDate,
            ArrivalTime = request.ArrivalTime,
            DepartureTime = request.DepartureTime,
            GuestName = request.GuestName.Trim(),
            GuestPhoneNumber = request.GuestPhoneNumber.Trim(),
            GuestCount = request.GuestCount,
            Total = nights * roomType.DailyPrice,
            Currency = roomType.Currency,
            IsCanceled = false
        };

        _reservationRepository.Add(reservation);
        return reservation;
    }

    public IReadOnlyCollection<Reservation> GetAll(ReservationFilter? filter)
    {
        return _reservationRepository.GetByFilter(filter ?? new ReservationFilter());
    }

    public Reservation GetById(Guid id)
    {
        return _reservationRepository.GetById(id)
            ?? throw new NotFoundException($"Reservation with id '{id}' was not found.");
    }

    public void Cancel(Guid id)
    {
        var reservation = _reservationRepository.GetById(id)
            ?? throw new NotFoundException($"Reservation with id '{id}' was not found.");

        if (reservation.IsCanceled)
        {
            return;
        }

        reservation.IsCanceled = true;
        _reservationRepository.Update(reservation);
    }

    private static void ValidateRequest(CreateReservationRequest request)
    {
        if (request == null)
        {
            throw new ValidationException("Reservation request is required.");
        }

        if (request.ArrivalDate >= request.DepartureDate)
        {
            throw new ValidationException("ArrivalDate must be earlier than DepartureDate.");
        }

        if (request.GuestCount <= 0)
        {
            throw new ValidationException("GuestCount must be greater than zero.");
        }

        if (string.IsNullOrWhiteSpace(request.GuestName))
        {
            throw new ValidationException("GuestName is required.");
        }

        if (string.IsNullOrWhiteSpace(request.GuestPhoneNumber))
        {
            throw new ValidationException("GuestPhoneNumber is required.");
        }
    }
}