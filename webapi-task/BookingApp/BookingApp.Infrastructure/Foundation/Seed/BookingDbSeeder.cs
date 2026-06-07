using BookingApp.Domain.Entities;

namespace BookingApp.Infrastructure.Foundation.Seed
{
    public static class BookingDbSeeder
    {
        private static readonly Guid GrandHotelId = Guid.Parse( "11111111-1111-1111-1111-111111111111" );
        private static readonly Guid RiversideApartHotelId = Guid.Parse( "22222222-2222-2222-2222-222222222222" );
        private static readonly Guid NevskyApartmentsId = Guid.Parse( "33333333-3333-3333-3333-333333333333" );
        private static readonly Guid KazanKremlinHotelId = Guid.Parse( "44444444-4444-4444-4444-444444444444" );

        private static readonly Guid GrandStandardId = Guid.Parse( "aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa1" );
        private static readonly Guid GrandSuiteId = Guid.Parse( "aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa2" );
        private static readonly Guid RiversideStudioId = Guid.Parse( "bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb1" );
        private static readonly Guid RiversideFamilyId = Guid.Parse( "bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb2" );
        private static readonly Guid NevskyStudioId = Guid.Parse( "cccccccc-cccc-cccc-cccc-ccccccccccc1" );
        private static readonly Guid NevskyDeluxeId = Guid.Parse( "cccccccc-cccc-cccc-cccc-ccccccccccc2" );
        private static readonly Guid KazanStandardId = Guid.Parse( "dddddddd-dddd-dddd-dddd-ddddddddddd1" );
        private static readonly Guid KazanSuiteId = Guid.Parse( "dddddddd-dddd-dddd-dddd-ddddddddddd2" );

        private static readonly Guid GrandStandardActiveReservationId = Guid.Parse( "eeeeeeee-eeee-eeee-eeee-eeeeeeeeeee1" );
        private static readonly Guid GrandStandardCanceledReservationId = Guid.Parse( "eeeeeeee-eeee-eeee-eeee-eeeeeeeeeee2" );
        private static readonly Guid GrandSuiteActiveReservationId = Guid.Parse( "eeeeeeee-eeee-eeee-eeee-eeeeeeeeeee3" );
        private static readonly Guid RiversideFamilyReservationId = Guid.Parse( "eeeeeeee-eeee-eeee-eeee-eeeeeeeeeee4" );
        private static readonly Guid NevskyStudioReservationId = Guid.Parse( "eeeeeeee-eeee-eeee-eeee-eeeeeeeeeee5" );
        private static readonly Guid KazanSuiteReservationId = Guid.Parse( "eeeeeeee-eeee-eeee-eeee-eeeeeeeeeee6" );

        public static void Seed( BookingDbContext dbContext )
        {
            IReadOnlyCollection<Property> properties = CreateProperties();
            IReadOnlyCollection<RoomType> roomTypes = CreateRoomTypes();
            IReadOnlyCollection<Reservation> reservations = CreateReservations( roomTypes );

            AddMissingProperties( dbContext, properties );
            AddMissingRoomTypes( dbContext, roomTypes );
            AddMissingReservations( dbContext, reservations );

            dbContext.SaveChanges();
        }

        private static IReadOnlyCollection<Property> CreateProperties()
        {
            return [
                new Property {
                            Id = GrandHotelId,
                                Name = "Grand Hotel",
                                Country = "Russia",
                                City = "Moscow",
                                Address = "Tverskaya Street, 1",
                                Latitude = 55.755800m,
                                Longitude = 37.617600m
                        },
                        new Property {
                            Id = RiversideApartHotelId,
                                Name = "Riverside Apart Hotel",
                                Country = "Russia",
                                City = "Moscow",
                                Address = "Prechistenskaya Embankment, 17",
                                Latitude = 55.738700m,
                                Longitude = 37.596600m
                        },
                        new Property {
                            Id = NevskyApartmentsId,
                                Name = "Nevsky Apartments",
                                Country = "Russia",
                                City = "Saint Petersburg",
                                Address = "Nevsky Prospect, 10",
                                Latitude = 59.934300m,
                                Longitude = 30.335100m
                        },
                        new Property {
                            Id = KazanKremlinHotelId,
                                Name = "Kazan Kremlin Hotel",
                                Country = "Russia",
                                City = "Kazan",
                                Address = "Kremlyovskaya Street, 5",
                                Latitude = 55.796300m,
                                Longitude = 49.108800m
                        }
            ];
        }

        private static IReadOnlyCollection<RoomType> CreateRoomTypes()
        {
            return [
                new RoomType {
                            Id = GrandStandardId,
                                PropertyId = GrandHotelId,
                                Name = "Standard",
                                DailyPrice = 5_000m,
                                Currency = "RUB",
                                MinPersonCount = 1,
                                MaxPersonCount = 2,
                                TotalRoomsCount = 2,
                                Services = ["Breakfast", "Wi-Fi"],
                                Amenities = ["TV", "Air conditioning", "Safe"]
                        },
                        new RoomType {
                            Id = GrandSuiteId,
                                PropertyId = GrandHotelId,
                                Name = "Suite",
                                DailyPrice = 12_000m,
                                Currency = "RUB",
                                MinPersonCount = 1,
                                MaxPersonCount = 4,
                                TotalRoomsCount = 1,
                                Services = ["Breakfast", "Parking", "Wi-Fi", "Late checkout"],
                                Amenities = ["TV", "Air conditioning", "Mini bar", "City view"]
                        },
                        new RoomType {
                            Id = RiversideStudioId,
                                PropertyId = RiversideApartHotelId,
                                Name = "Studio",
                                DailyPrice = 6_500m,
                                Currency = "RUB",
                                MinPersonCount = 1,
                                MaxPersonCount = 2,
                                TotalRoomsCount = 4,
                                Services = ["Wi-Fi", "Self check-in"],
                                Amenities = ["Kitchenette", "Washing machine", "Workspace"]
                        },
                        new RoomType {
                            Id = RiversideFamilyId,
                                PropertyId = RiversideApartHotelId,
                                Name = "Family Apartment",
                                DailyPrice = 9_500m,
                                Currency = "RUB",
                                MinPersonCount = 2,
                                MaxPersonCount = 5,
                                TotalRoomsCount = 2,
                                Services = ["Wi-Fi", "Self check-in", "Parking"],
                                Amenities = ["Kitchen", "Washing machine", "Baby cot", "River view"]
                        },
                        new RoomType {
                            Id = NevskyStudioId,
                                PropertyId = NevskyApartmentsId,
                                Name = "Studio",
                                DailyPrice = 7_000m,
                                Currency = "RUB",
                                MinPersonCount = 1,
                                MaxPersonCount = 3,
                                TotalRoomsCount = 3,
                                Services = ["Wi-Fi"],
                                Amenities = ["Kitchen", "Washing machine", "Heating"]
                        },
                        new RoomType {
                            Id = NevskyDeluxeId,
                                PropertyId = NevskyApartmentsId,
                                Name = "Deluxe Apartment",
                                DailyPrice = 11_000m,
                                Currency = "RUB",
                                MinPersonCount = 1,
                                MaxPersonCount = 4,
                                TotalRoomsCount = 2,
                                Services = ["Wi-Fi", "Breakfast delivery"],
                                Amenities = ["Kitchen", "Washing machine", "Balcony", "Nevsky view"]
                        },
                        new RoomType {
                            Id = KazanStandardId,
                                PropertyId = KazanKremlinHotelId,
                                Name = "Standard",
                                DailyPrice = 4_500m,
                                Currency = "RUB",
                                MinPersonCount = 1,
                                MaxPersonCount = 2,
                                TotalRoomsCount = 5,
                                Services = ["Breakfast", "Wi-Fi"],
                                Amenities = ["TV", "Air conditioning"]
                        },
                        new RoomType {
                            Id = KazanSuiteId,
                                PropertyId = KazanKremlinHotelId,
                                Name = "Kremlin View Suite",
                                DailyPrice = 10_000m,
                                Currency = "RUB",
                                MinPersonCount = 1,
                                MaxPersonCount = 4,
                                TotalRoomsCount = 1,
                                Services = ["Breakfast", "Parking", "Wi-Fi"],
                                Amenities = ["TV", "Air conditioning", "Mini bar", "Kremlin view"]
                        }
            ];
        }

        private static IReadOnlyCollection<Reservation> CreateReservations( IReadOnlyCollection<RoomType> roomTypes )
        {
            RoomType grandStandard = GetRoomType( roomTypes, GrandStandardId );
            RoomType grandSuite = GetRoomType( roomTypes, GrandSuiteId );
            RoomType riversideFamily = GetRoomType( roomTypes, RiversideFamilyId );
            RoomType nevskyStudio = GetRoomType( roomTypes, NevskyStudioId );
            RoomType kazanSuite = GetRoomType( roomTypes, KazanSuiteId );

            return [
                CreateReservation(
                            GrandStandardActiveReservationId,
                            GrandHotelId,
                            grandStandard,
                            new DateOnly(2026, 7, 10),
                            new DateOnly(2026, 7, 15),
                            "Ivan Ivanov",
                            "+79990000001",
                            guestCount: 2,
                            isCanceled: false),
                        CreateReservation(
                            GrandStandardCanceledReservationId,
                            GrandHotelId,
                            grandStandard,
                            new DateOnly(2026, 7, 10),
                            new DateOnly(2026, 7, 15),
                            "Maria Petrova",
                            "+79990000002",
                            guestCount: 1,
                            isCanceled: true),
                        CreateReservation(
                            GrandSuiteActiveReservationId,
                            GrandHotelId,
                            grandSuite,
                            new DateOnly(2026, 7, 10),
                            new DateOnly(2026, 7, 15),
                            "Pavel Sidorov",
                            "+79990000003",
                            guestCount: 3,
                            isCanceled: false),
                        CreateReservation(
                            RiversideFamilyReservationId,
                            RiversideApartHotelId,
                            riversideFamily,
                            new DateOnly(2026, 7, 12),
                            new DateOnly(2026, 7, 18),
                            "Anna Smirnova",
                            "+79990000004",
                            guestCount: 4,
                            isCanceled: false),
                        CreateReservation(
                            NevskyStudioReservationId,
                            NevskyApartmentsId,
                            nevskyStudio,
                            new DateOnly(2026, 8, 5),
                            new DateOnly(2026, 8, 10),
                            "Sergey Volkov",
                            "+79990000005",
                            guestCount: 2,
                            isCanceled: false),
                        CreateReservation(
                            KazanSuiteReservationId,
                            KazanKremlinHotelId,
                            kazanSuite,
                            new DateOnly(2026, 9, 1),
                            new DateOnly(2026, 9, 5),
                            "Elena Kuznetsova",
                            "+79990000006",
                            guestCount: 2,
                            isCanceled: false)
            ];
        }

        private static RoomType GetRoomType( IReadOnlyCollection<RoomType> roomTypes, Guid id )
        {
            return roomTypes.Single( roomType => roomType.Id == id );
        }

        private static Reservation CreateReservation(
            Guid id,
            Guid propertyId,
            RoomType roomType,
            DateOnly arrivalDate,
            DateOnly departureDate,
            string guestName,
            string guestPhoneNumber,
            int guestCount,
            bool isCanceled )
        {
            return new Reservation
            {
                Id = id,
                PropertyId = propertyId,
                RoomTypeId = roomType.Id,
                ArrivalDate = arrivalDate,
                DepartureDate = departureDate,
                ArrivalTime = new TimeOnly( 14, 0 ),
                DepartureTime = new TimeOnly( 12, 0 ),
                GuestName = guestName,
                GuestPhoneNumber = guestPhoneNumber,
                GuestCount = guestCount,
                Total = CalculateTotal( roomType.DailyPrice, arrivalDate, departureDate ),
                Currency = roomType.Currency,
                IsCanceled = isCanceled
            };
        }

        private static decimal CalculateTotal( decimal dailyPrice, DateOnly arrivalDate, DateOnly departureDate )
        {
            int nights = departureDate.DayNumber - arrivalDate.DayNumber;
            return dailyPrice * nights;
        }

        private static void AddMissingProperties( BookingDbContext dbContext, IReadOnlyCollection<Property> properties )
        {
            HashSet<Guid> existingIds = dbContext.Set<Property>().Select( property => property.Id ).ToHashSet();
            List<Property> missingProperties = properties.Where( property => !existingIds.Contains( property.Id ) ).ToList();

            if ( missingProperties.Count > 0 )
            {
                dbContext.Set<Property>().AddRange( missingProperties );
            }
        }

        private static void AddMissingRoomTypes( BookingDbContext dbContext, IReadOnlyCollection<RoomType> roomTypes )
        {
            HashSet<Guid> existingIds = dbContext.Set<RoomType>().Select( roomType => roomType.Id ).ToHashSet();
            List<RoomType> missingRoomTypes = roomTypes.Where( roomType => !existingIds.Contains( roomType.Id ) ).ToList();

            if ( missingRoomTypes.Count > 0 )
            {
                dbContext.Set<RoomType>().AddRange( missingRoomTypes );
            }
        }

        private static void AddMissingReservations( BookingDbContext dbContext, IReadOnlyCollection<Reservation> reservations )
        {
            HashSet<Guid> existingIds = dbContext.Set<Reservation>().Select( reservation => reservation.Id ).ToHashSet();
            List<Reservation> missingReservations = reservations.Where( reservation => !existingIds.Contains( reservation.Id ) ).ToList();

            if ( missingReservations.Count > 0 )
            {
                dbContext.Set<Reservation>().AddRange( missingReservations );
            }
        }
    }
}