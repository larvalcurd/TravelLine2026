namespace BookingApp.WebApi.DTOs.RoomTypes
{
    public class RoomTypeDto
    {
        public Guid Id { get; set; }
        public Guid PropertyId { get; set; }
        public required string Name { get; set; }
        public decimal DailyPrice { get; set; }
        public required string Currency { get; set; }
        public int MinPersonCount { get; set; }
        public int MaxPersonCount { get; set; }
        public int TotalRoomsCount { get; set; }
        public IReadOnlyCollection<string> Services { get; set; } = [];
        public IReadOnlyCollection<string> Amenities { get; set; } = [];
    }
}
