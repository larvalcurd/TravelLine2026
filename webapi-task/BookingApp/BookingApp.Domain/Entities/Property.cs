namespace BookingApp.Domain.Entities
{
    public class Property
    {
        public Guid Id { get; set; }

        public required string Name { get; set; }
        public required string Country { get; set; }
        public required string City { get; set; }
        public required string Address { get; set; }

        public required decimal Latitude { get; set; }
        public required decimal Longitude { get; set; }

        public ICollection<RoomType> RoomTypes { get; set; } = [];

        public void Update(
            string name,
            string country,
            string city,
            string address,
            decimal latitude,
            decimal longitude )
        {
            Name = name;
            Country = country;
            City = city;
            Address = address;
            Latitude = latitude;
            Longitude = longitude;
        }
    }
}