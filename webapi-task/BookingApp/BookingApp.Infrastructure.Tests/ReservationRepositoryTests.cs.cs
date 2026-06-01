using BookingApp.Domain.Entities;
using BookingApp.Domain.Models;
using BookingApp.Infrastructure.Persistence;
using BookingApp.Infrastructure.Repositories;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace BookingApp.Infrastructure.Tests
{
    public class ReservationRepositoryTests : IDisposable
    {
        private readonly SqliteConnection _connection;
        private readonly BookingDbContext _context;
        private readonly EfReservationRepository _repository;

        private readonly Guid _propertyId = Guid.NewGuid();
        private readonly Guid _roomTypeId = Guid.NewGuid();

        public ReservationRepositoryTests()
        {
            _connection = new SqliteConnection( "DataSource=:memory:" );
            _connection.Open();

            var options = new DbContextOptionsBuilder<BookingDbContext>()
                .UseSqlite( _connection )
                .Options;

            _context = new BookingDbContext( options );
            _context.Database.EnsureCreated();

            SeedParentEntities();

            _repository = new EfReservationRepository( _context );
        }

        private void SeedParentEntities()
        {
            var property = new Property
            {
                Id = _propertyId,
                Name = "Тестовый Отель",
                Country = "Россия",
                City = "Йошкар-Ола",
                Address = "ул. Кремлевская, д. 1",
                Latitude = 56.632m,
                Longitude = 47.889m
            };

            var roomType = new RoomType
            {
                Id = _roomTypeId,
                PropertyId = _propertyId,
                Name = "Стандарт",
                Currency = "RUB",
                DailyPrice = 3000,
                MinPersonCount = 1,
                MaxPersonCount = 2,
                TotalRoomsCount = 5,
                Services = [ "Wi-Fi" ],
                Amenities = [ "Кровать" ]
            };

            _context.Properties.Add( property );
            _context.RoomTypes.Add( roomType );
            _context.SaveChanges();
        }

        [Fact]
        public void Overlapping_ShouldHandle_DateBordersCorrectly()
        {
            var existing = CreateReservation( new DateOnly( 2026, 5, 10 ), new DateOnly( 2026, 5, 12 ) );
            _repository.Add( existing );

            var count1 = _repository.GetOverlappingCount( _roomTypeId, new DateOnly( 2026, 5, 12 ), new DateOnly( 2026, 5, 14 ) );
            Assert.Equal( 0, count1 );

            var count2 = _repository.GetOverlappingCount( _roomTypeId, new DateOnly( 2026, 5, 11 ), new DateOnly( 2026, 5, 14 ) );
            Assert.Equal( 1, count2 );
        }

        [Fact]
        public void GetOverlappingCount_ShouldIgnore_CanceledReservations()
        {
            var canceled = CreateReservation( new DateOnly( 2026, 5, 10 ), new DateOnly( 2026, 5, 15 ) );
            canceled.IsCanceled = true;
            _repository.Add( canceled );

            var count = _repository.GetOverlappingCount( _roomTypeId, new DateOnly( 2026, 5, 12 ), new DateOnly( 2026, 5, 14 ) );

            Assert.Equal( 0, count );
        }

        [Fact]
        public void GetByFilter_ShouldFilter_AllCriteriaCorrectly()
        {
            var res1 = CreateReservation( new DateOnly( 2026, 5, 10 ), new DateOnly( 2026, 5, 15 ) );
            res1.GuestName = "Kiki";
            res1.GuestPhoneNumber = "+79991112233";
            _repository.Add( res1 );

            var res2 = CreateReservation( new DateOnly( 2026, 5, 10 ), new DateOnly( 2026, 5, 15 ) );
            res2.GuestName = "Bob";
            res2.IsCanceled = true;
            _repository.Add( res2 );

            var filter = new ReservationFilter
            {
                IncludeCanceled = false,
                GuestName = "kiki",
                PropertyId = _propertyId
            };
            var result = _repository.GetByFilter( filter );

            Assert.Single( result );
            Assert.Equal( "Kiki", result.First().GuestName );
        }

        [Fact]
        public void GetOverlappingCount_ShouldFilter_ByRoomTypeIds()
        {
            var otherRoomType = new RoomType
            {
                Id = Guid.NewGuid(),
                PropertyId = _propertyId,
                Name = "Люкс",
                Currency = "RUB",
                DailyPrice = 5000,
                MinPersonCount = 1,
                MaxPersonCount = 4,
                TotalRoomsCount = 2
            };

            _context.RoomTypes.Add( otherRoomType );
            _context.SaveChanges();

            var r1 = CreateReservation( new DateOnly( 2026, 5, 10 ), new DateOnly( 2026, 5, 15 ) );

            var r2 = CreateReservation( new DateOnly( 2026, 5, 10 ), new DateOnly( 2026, 5, 15 ) );

            r2.RoomTypeId = otherRoomType.Id;

            _repository.Add( r1 );
            _repository.Add( r2 );

            var result = _repository.GetOverlappingReservations( [ _roomTypeId ], new DateOnly( 2026, 5, 12 ), new DateOnly( 2026, 5, 14 ) );

            Assert.Single( result );
        }

        [Fact]
        public void GetByFilter_ShouldFilter_ByDateRange()
        {
            var res1 = CreateReservation(
                new DateOnly( 2026, 5, 10 ),
                new DateOnly( 2026, 5, 15 ) );

            var res2 = CreateReservation(
                new DateOnly( 2026, 6, 10 ),
                new DateOnly( 2026, 6, 15 ) );

            _repository.Add( res1 );
            _repository.Add( res2 );

            var filter = new ReservationFilter
            {
                ArrivalDateFrom = new DateOnly( 2026, 5, 1 ),
                ArrivalDateTo = new DateOnly( 2026, 5, 31 )
            };

            var result = _repository.GetByFilter( filter );

            Assert.Single( result );
        }

        private Reservation CreateReservation( DateOnly arrival, DateOnly departure )
        {
            return new Reservation
            {
                Id = Guid.NewGuid(),
                PropertyId = _propertyId,
                RoomTypeId = _roomTypeId,
                ArrivalDate = arrival,
                DepartureDate = departure,
                ArrivalTime = new TimeOnly( 14, 0 ),
                DepartureTime = new TimeOnly( 12, 0 ),
                GuestName = "Test",
                GuestPhoneNumber = "123",
                Currency = "RUB",
                Total = 5000,
                IsCanceled = false
            };
        }

        public void Dispose()
        {
            _context.Dispose();
            _connection.Dispose();
        }
    }
}