namespace BookingApp.WebApi.DTOs.Reservations
{
    public class CreateReservationDto
    {
        public Guid PropertyId { get; set; }
        public Guid RoomTypeId { get; set; }

        public DateOnly ArrivalDate { get; set; }
        public DateOnly DepartureDate { get; set; }
        public TimeOnly ArrivalTime { get; set; }
        public TimeOnly DepartureTime { get; set; }

        public required string GuestName { get; set; }
        public required string GuestPhoneNumber { get; set; }
        public int GuestCount { get; set; }
    }
}