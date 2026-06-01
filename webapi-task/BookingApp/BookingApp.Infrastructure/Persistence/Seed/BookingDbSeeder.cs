

using BookingApp.Domain.Entities;

namespace BookingApp.Infrastructure.Persistence.Seed
{
    public static class BookingDbSeeder
    {
        public static void Seed( BookingDbContext dbContext )
        {
            if ( dbContext.Properties.Any() )
            {
                return;
            }

            var property1Id = Guid.NewGuid();
            var property2Id = Guid.NewGuid();

            var roomType1Id = Guid.NewGuid();
            var roomType2Id = Guid.NewGuid();
            var roomType3Id = Guid.NewGuid();

            var property1 = new Property
            {
                Id = property1Id,
                Name = "Grand Hotel",
                Country = "Russia",
                City = "Moscow",
                Address = "Tverskaya 1",
                Latitude = 55.7558m,
                Longitude = 37.6176m
            };

            var property2 = new Property
            {
                Id = property2Id,
                Name = "Nevsky Apartments",
                Country = "Russia",
                City = "Saint Petersburg",
                Address = "Nevsky Prospect 10",
                Latitude = 59.9343m,
                Longitude = 30.3351m
            };

            var roomType1 = new RoomType
            {
                Id = roomType1Id,
                PropertyId = property1Id,
                Name = "Standard",
                DailyPrice = 5000,
                Currency = "RUB",
                MinPersonCount = 1,
                MaxPersonCount = 2,
                TotalRoomsCount = 10,
                Services = [ "Breakfast", "WiFi" ],
                Amenities = [ "TV", "Air conditioning" ]
            };

            var roomType2 = new RoomType
            {
                Id = roomType2Id,
                PropertyId = property1Id,
                Name = "Suite",
                DailyPrice = 12000,
                Currency = "RUB",
                MinPersonCount = 1,
                MaxPersonCount = 4,
                TotalRoomsCount = 3,
                Services = [ "Breakfast", "Parking", "WiFi" ],
                Amenities = [ "TV", "Air conditioning", "Mini bar" ]
            };

            var roomType3 = new RoomType
            {
                Id = roomType3Id,
                PropertyId = property2Id,
                Name = "Studio",
                DailyPrice = 7000,
                Currency = "RUB",
                MinPersonCount = 1,
                MaxPersonCount = 3,
                TotalRoomsCount = 5,
                Services = [ "WiFi" ],
                Amenities = [ "Kitchen", "Washing machine" ]
            };

            var reservation1 = new Reservation
            {
                Id = Guid.NewGuid(),
                PropertyId = property1Id,
                RoomTypeId = roomType1Id,
                ArrivalDate = new DateOnly( 2025, 6, 10 ),
                DepartureDate = new DateOnly( 2025, 6, 15 ),
                ArrivalTime = new TimeOnly( 14, 0 ),
                DepartureTime = new TimeOnly( 12, 0 ),
                GuestName = "Ivan Ivanov",
                GuestPhoneNumber = "+79990000001",
                GuestCount = 2,
                Total = 25000,
                Currency = "RUB",
                IsCanceled = false
            };

            dbContext.Properties.AddRange( property1, property2 );
            dbContext.RoomTypes.AddRange( roomType1, roomType2, roomType3 );
            dbContext.Reservations.Add( reservation1 );

            dbContext.SaveChanges();
        }
    }
}