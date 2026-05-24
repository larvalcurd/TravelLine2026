namespace BookingApp.Domain.Entities
{
    public class Reservation
    {
        public Guid Id { get; set; }

        public Guid PropertyId { get; set; }
        public Guid RoomTypeId { get; set; } 

        public DateTime CheckInDateTime { get; set; }
        public DateTime CheckOutDateTime { get; set; }

        public required string GuestName { get; set; }
        public required string GuestPhoneNumber { get; set; }

        public decimal Total { get; set; }
        public required string Currency { get; set; }

        public Property? Property { get; set; }
        public RoomType? RoomType { get; set; }
    }
}