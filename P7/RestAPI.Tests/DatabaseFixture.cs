using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using RestAPI.Models;
using RestAPI.Tests.Properties;
using Xunit;
//[assembly: CollectionBehavior(DisableTestParallelization = true)]

namespace RestAPI.Tests
{
    [CollectionDefinition("SharedDbContext")]
    public class DatabaseCollection : ICollectionFixture<DatabaseFixture> { }

    public class DatabaseFixture : IDisposable
    {
        public ApplicationDbContext ApplicationDbContext;
        public ApplicationDbContext InMemoryApplicationDbContext;


        public DatabaseFixture()
        {
            // Test for real database READ
            var contextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlServer(Resources.ConnectionString)
                .Options;
            //// Test InMemory CREATE UPDATE DELETE
            var inMemoryContextOptions = DbContextOptionsBuilder();

            ApplicationDbContext = new ApplicationDbContext(contextOptions);
            InMemoryApplicationDbContext = new ApplicationDbContext(inMemoryContextOptions);

            SeedInMemoryTestDb(inMemoryContextOptions);
        }

        private static DbContextOptions<ApplicationDbContext> DbContextOptionsBuilder()
        {
            return new DbContextOptionsBuilder<ApplicationDbContext>()
                .EnableSensitiveDataLogging()
                .UseInMemoryDatabase(Guid.NewGuid().ToString(), new InMemoryDatabaseRoot())
                .Options;
        }

        private void SeedInMemoryTestDb(DbContextOptions<ApplicationDbContext> options)
        {
            //InMemoryApplicationDbContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            var bids = new List<Bid>
                {
                    new(){Account = "1"},
                    new (){Account = "2"},
                    new (){Account = "3"},
                    new (){Account = "4"}
                };
            InMemoryApplicationDbContext.Bids.AddRange(bids);

            var curvePoints = new List<CurvePoint>
            {
                new(),
                new (),
                new (),
                new ()
            };
            InMemoryApplicationDbContext.CurvePoints.AddRange(curvePoints);

            var ratings = new List<Rating>
            {
                new(){MoodyRating = "Unit Test"},
                new (){MoodyRating = "Unit Test"},
                new (){MoodyRating = "Unit Test"},
                new (){MoodyRating = "Unit Test"}
            };
            InMemoryApplicationDbContext.Ratings.AddRange(ratings);

            var rules = new List<Rule>
            {
                new(){Description = "Unit Test"},
                new (){Description = "Unit Test"},
                new (){Description = "Unit Test"},
                new (){Description = "Unit Test"}
            };
            InMemoryApplicationDbContext.Rules.AddRange(rules);

            var trades = new List<Trade>
            {
                new(){Account = "Unit Test"},
                new (){Account = "Unit Test"},
                new (){Account = "Unit Test"},
                new (){Account = "Unit Test"}
            };
            InMemoryApplicationDbContext.Trades.AddRange(trades);

            InMemoryApplicationDbContext.SaveChanges();
        }

        public void Dispose()
        {
            ApplicationDbContext.Dispose();
            InMemoryApplicationDbContext.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}