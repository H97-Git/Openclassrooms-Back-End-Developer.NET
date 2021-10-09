using System;
using System.Collections.Generic;
using System.Linq;
using Serilog;

namespace CalifornianHealth.Booking.Data
{
    public class DbInitializer
    {
        private static List<Availability> _availabilities;

        public static void Initialize(BookingContext context)
        {
            _availabilities = new List<Availability>();
            Log.Information("DbInitializer : EnsureCreated()");
            context.Database.EnsureCreated();

            if (context.Availabilities.Any() || context.Bookings.Any())
            {
                Log.Information("DbInitializer : DB Already seeded");

                Log.Information("DbInitializer : Clean up availabilities");
                foreach (var availability in context.Availabilities.Where(availability =>
                    availability.DateTime < DateTime.Now)) context.Availabilities.Remove(availability);

                context.SaveChanges();
                return;
            }

            Log.Information("DbInitializer : GenerateAvailability()");
            GenerateAvailability();

            context.Availabilities.AddRange(_availabilities);

            Log.Information("DbInitializer : SaveChanges()");
            context.SaveChanges();
        }

        private static void GenerateAvailability()
        {
            var now = DateTime.Now;
            var lastDayOfMonth = GetLastDayOfMonth(now.AddMonths(1));

            var result = DateTime.Compare(now.Date, lastDayOfMonth);
            var index = 0;
            while (result == -1)
            {
                index++;
                var offset = new TimeSpan(index, 0, 0, 0);
                var comparingDate = now.Date.Add(offset);
                result = DateTime.Compare(comparingDate, lastDayOfMonth);

                if (comparingDate.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday) continue;

                for (var i = 1; i <= 4; i++)
                {
                    var availabilities = CreateAvailabilities(i, comparingDate);
                    _availabilities.AddRange(availabilities);
                }
            }
        }

        private static DateTime GetLastDayOfMonth(DateTime now)
        {
            //var startOfMonth = new DateTime(now.Year, now.Month, 1);
            var daysInMonth = DateTime.DaysInMonth(now.Year, now.Month);
            return new DateTime(now.Year, now.Month, daysInMonth);
        }

        private static IEnumerable<Availability> CreateAvailabilities(int consultantId, DateTime date)
        {
            return new List<Availability>
            {
                new()
                {
                    ConsultantId = consultantId,
                    DateTime = date.AddHours(GetRandomHours(10, 13))
                },
                new()
                {
                    ConsultantId = consultantId,
                    DateTime = date.AddHours(GetRandomHours(13, 17))
                },
                new()
                {
                    ConsultantId = consultantId,
                    DateTime = date.AddHours(GetRandomHours(17))
                }
            };
        }

        private static int GetRandomHours(int start = 10, int end = 20)
        {
            var rnd = new Random();
            return rnd.Next(start, end);
        }
    }
}