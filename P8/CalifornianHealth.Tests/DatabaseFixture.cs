using System;
using System.Collections.Generic;
using Bogus;
using CalifornianHealth.Booking.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Xunit;

namespace CalifornianHealth.Tests
{
    [CollectionDefinition("SharedDbContext")]
    public class DatabaseCollection : ICollectionFixture<DatabaseFixture>
    {
    }

    public class DatabaseFixture : IDisposable
    {
        public readonly BookingContext Context;

        public DatabaseFixture()
        {
            var contextOptionsBuilder = DbContextOptionsBuilder();
            Context = new BookingContext(contextOptionsBuilder);
            SeedAvailabilityDb();
            SeedBookingDb();
        }

        public void Dispose()
        {
            Context?.Dispose();
            GC.SuppressFinalize(this);
        }

        private static DbContextOptions<BookingContext> DbContextOptionsBuilder()
        {
            return new DbContextOptionsBuilder<BookingContext>()
                .EnableSensitiveDataLogging()
                .UseInMemoryDatabase(Guid.NewGuid().ToString(), new InMemoryDatabaseRoot())
                .Options;
        }

        private void SeedAvailabilityDb()
        {
            var faker = new Faker<Availability>()
                .Rules((f, a) =>
                {
                    a.ConsultantId = f.Random.Int(1, 4);
                    a.DateTime = f.Date.Soon();
                });
            var availabilities = new List<Availability>();
            for (var i = 0; i < 599; i++)
            {
                var availability = faker.Generate();
                availabilities.Add(availability);
            }

            Context.Availabilities.AddRange(availabilities);
            Context.SaveChanges();
        }

        private void SeedBookingDb()
        {
            var faker = new Faker<Booking.Data.Booking>()
                .Rules((f, b) =>
                {
                    b.Appointment = f.Date.Soon();
                    b.AvailabilityId = f.Random.Int(1, 599);
                    b.ConsultantId = f.Random.Int(1, 4);
                });
            var bookings = new List<Booking.Data.Booking>();
            for (var i = 0; i < 599; i++)
            {
                var booking = faker.Generate();
                bookings.Add(booking);
            }

            Context.Bookings.AddRange(bookings);
            Context.SaveChanges();
        }
    }
}