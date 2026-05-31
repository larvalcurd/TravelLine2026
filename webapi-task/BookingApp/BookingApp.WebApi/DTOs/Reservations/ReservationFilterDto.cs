namespace BookingApp.WebApi.DTOs.Reservations
{
    public class ReservationFilterDto
    {
        public Guid? PropertyId { get; set; }
        public Guid? RoomTypeId { get; set; }
        public DateOnly? ArrivalDateFrom { get; set; }
        public DateOnly? ArrivalDateTo { get; set; }
        public string? GuestName { get; set; }
        public string? GuestPhoneNumber { get; set; }
        public bool IncludeCanceled { get; set; }
    }
}