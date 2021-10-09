using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using P3AddNewFunctionalityDotNetCore.Data;
using P3AddNewFunctionalityDotNetCore.Models;
using P3AddNewFunctionalityDotNetCore.Models.Entities;
using P3AddNewFunctionalityDotNetCore.Models.Repositories;
using P3AddNewFunctionalityDotNetCore.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using P3AddNewFunctionalityDotNetCore.Models.Services;
using Xunit;

namespace P3AddNewFunctionalityDotNetCore.Tests.Models.Repositories
{
    [Collection("Sequential")]
    public class OrderRepositoryTests
    {
        // In Memory Database Test
        private DbContextOptions<P3Referential> DbContextOptionsBuilder()
        {
            return new DbContextOptionsBuilder<P3Referential>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString(), new InMemoryDatabaseRoot())
                .Options;
        }

        public Order CreateOrder(DateTime now)
        {
            var order = new Order
            {
                Address = "Address",
                City = "City",
                Country = "Country",
                Date = now,
                Name = "Name",
                Zip = "00000",
                OrderLine = new List<OrderLine>()
            };
            OrderLine orderLine = new OrderLine
            {
                ProductId = 1,
                Quantity = 1,
            };
            order.OrderLine.Add(orderLine);

            return order;
        }
        
        [Fact]
        public void Save_GoodPath()
        {
            var options = DbContextOptionsBuilder();
            using (var context = new P3Referential(options))
            {
                
                var orderRepository = new OrderRepository(context);

                var dateNow = DateTime.Now;

                var order = CreateOrder(dateNow);

                orderRepository.Save(order);

                Assert.NotNull(context.Order.First());
                Assert.Equal(dateNow, context.Order.First().Date);
            }
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Save_OrderNull_EmptyOrderLine_BadPath(bool nullTest)
        {
            var options = DbContextOptionsBuilder();
            using (var context = new P3Referential(options))
            {

                var orderRepository = new OrderRepository(context);

                var order = new Order
                {
                    Address = "Address",
                    City = "City",
                    Country = "Country",
                    Date = DateTime.Now,
                    Name = "Name",
                    Zip = "00000",
                    OrderLine = new List<OrderLine>()
                };
                if (nullTest)
                {
                    orderRepository.Save(null);
                    Assert.Empty(context.Order);
                }
                else
                {
                    orderRepository.Save(order);
                    Assert.Empty(context.Order);
                }
            }
        }
        [Fact]
        public void Save_contextNull_BadPath()
        {
            var options = DbContextOptionsBuilder();
            using (var context = new P3Referential(options))
            {
                var orderRepository = new OrderRepository(null);

                var order = new Order
                {
                    Address = "Address",
                    City = "City",
                    Country = "Country",
                    Date = DateTime.Now,
                    Name = "Name",
                    Zip = "00000",
                    OrderLine = new List<OrderLine>()
                };
               
                orderRepository.Save(order);
                Assert.Empty(context.Order);
                
            }
        }
        [Fact]
        public async void GetOrder_GoodPath()
        {
            var options = DbContextOptionsBuilder();
            using (var context = new P3Referential(options))
            {

                var orderRepository = new OrderRepository(context);

                var dateNow = DateTime.Now;
                var order = CreateOrder(dateNow);
                orderRepository.Save(order);

                var getOrder = await orderRepository.GetOrder(1);

                Assert.Equal(context.Order.First().Date, getOrder.Date);
            }
        }
        [Fact]
        public async void GetOrder_BadId_BadPath()
        {
            var options = DbContextOptionsBuilder();
            using (var context = new P3Referential(options))
            {

                var orderRepository = new OrderRepository(context);

                var dateNow = DateTime.Now;
                var order = CreateOrder(dateNow);
                orderRepository.Save(order);

                var getOrder = await orderRepository.GetOrder(999);

                Assert.Null(getOrder);
            }
        }
        [Fact]
        public async void GetOrder_contextNull_BadPath()
        {
            var orderRepository = new OrderRepository(null);

            var getOrder = await orderRepository.GetOrder(999);

            Assert.Null(getOrder);
        }
        [Fact]
        public async void GetOrders_GoodPath()
        {
            var options = DbContextOptionsBuilder();
            using (var context = new P3Referential(options))
            {

                var orderRepository = new OrderRepository(context);

                var dateNow = DateTime.Now;
                
                var order1 = CreateOrder(dateNow);
                orderRepository.Save(order1);

                var order2 = CreateOrder(dateNow);
                orderRepository.Save(order2);

                var getOrders = await orderRepository.GetOrders();
                Assert.Equal(2, getOrders.Count);
            }
        }
        [Fact]
        public async void GetOrders_BadPath()
        {
            var options = DbContextOptionsBuilder();
            using (var context = new P3Referential(options))
            {

                var orderRepository = new OrderRepository(context);

                var dateNow = DateTime.Now;

                var order1 = CreateOrder(dateNow);
                orderRepository.Save(order1);

                var order2 = CreateOrder(dateNow);
                orderRepository.Save(order2);

                var getOrders = await orderRepository.GetOrders();
                Assert.Equal(2, getOrders.Count);
            }
        }
    }
}
