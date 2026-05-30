namespace BookingApp.WebApi.DTOs.Properties
{
    public class CreatePropertyDto
    {
        public required string Name { get; set; }
        public required string Country { get; set; }
        public required string City { get; set; }
        public required string Address { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
    }
}