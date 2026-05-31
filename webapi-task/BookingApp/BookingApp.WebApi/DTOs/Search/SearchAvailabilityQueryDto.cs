namespace BookingApp.WebApi.DTOs.Search
{
    public class SearchAvailabilityQueryDto
    {
        public required string City { get; set; }
        public DateOnly ArrivalDate { get; set; }
        public DateOnly DepartureDate { get; set; }
        public int Guests { get; set; }
        public decimal? MaxPrice { get; set; }
    }
}