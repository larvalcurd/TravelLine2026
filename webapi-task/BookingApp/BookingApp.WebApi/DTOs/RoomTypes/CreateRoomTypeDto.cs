namespace BookingApp.WebApi.DTOs.RoomTypes
{
    public class CreateRoomTypeDto
    {
        public required string Name { get; set; } = string.Empty;
        public decimal DailyPrice { get; set; }
        public required string Currency { get; set; } = string.Empty;
        public int MinPersonCount { get; set; }
        public int MaxPersonCount { get; set; }
        public int TotalRoomsCount { get; set; }
        public IReadOnlyCollection<string> Services { get; set; } = [];
        public IReadOnlyCollection<string> Amenities { get; set; } = [];
    }
}
