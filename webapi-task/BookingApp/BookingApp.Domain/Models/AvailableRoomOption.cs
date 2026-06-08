namespace BookingApp.Domain.Models
{
    public class AvailableRoomOption
    {
        public Guid PropertyId { get; set; }
        public required string PropertyName { get; set; }
        public required string Country { get; set; }
        public required string City { get; set; }
        public required string Address { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }

        public Guid RoomTypeId { get; set; }
        public required string RoomTypeName { get; set; }
        public decimal DailyPrice { get; set; }
        public required string Currency { get; set; }
        public int MinPersonCount { get; set; }
        public int MaxPersonCount { get; set; }
        public int AvailableRoomsCount { get; set; }

        public int Nights { get; set; }
        public decimal TotalPrice { get; set; }

        public List<string> Services { get; set; } = [];
        public List<string> Amenities { get; set; } = [];
    }
}