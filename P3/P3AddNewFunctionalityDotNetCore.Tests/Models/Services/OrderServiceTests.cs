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

namespace P3AddNewFunctionalityDotNetCore.Tests.Models.Services
{
    [Collection("Sequential")]
    public class OrderServiceTests
    {
        private readonly P3Referential _context;
        private readonly OrderService _orderService;
        private readonly OrderViewModel _orderViewModel;
        public OrderServiceTests()
        {
            var options = DbContextOptionsBuilder();
            _context = new P3Referential(options);
            var productRepository = new ProductRepository(_context);
            var orderRepository = new OrderRepository(_context);
            SeedTestDb(options);
            var cart = new Cart();


            var product = productRepository.GetAllProducts().First();
            cart.AddItem(product, 1);

            var productService = new ProductService(cart, productRepository, orderRepository, null);
            _orderService = new OrderService(cart, orderRepository, productService);

            _orderViewModel = new OrderViewModel
            {
                Address = "Address",
                City = "City",
                Country = "Country",
                Date = DateTime.Now,
                Name = "OrderServiceName",
                Zip = "00000",
                Lines = ((Cart)cart)?.Lines.ToArray(),
            };
        }
        // In Memory Database Test
        private DbContextOptions<P3Referential> DbContextOptionsBuilder()
        {
            return new DbContextOptionsBuilder<P3Referential>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString(), new InMemoryDatabaseRoot())
                .Options;
        }

        private IEnumerable<Product> _testProductsList;
        private List<Product> CreateProductList()
        {
            _testProductsList = new List<Product>
            {
                new Product
                {
                    Name = "one name",
                    Description = "one description",
                    Details = "one details",
                    Quantity = 2,
                    Price = 10
                },
                new Product
                {
                    Name = "two name",
                    Description = "two description",
                    Details = "two details",
                    Quantity = 4,
                    Price = 20
                },
                new Product
                {
                    Name = "three name",
                    Description = "three description",
                    Details = "three details",
                    Quantity = 6,
                    Price = 30
                }
            };
            return _testProductsList.ToList();
        }

        private void SeedTestDb(DbContextOptions<P3Referential> options)
        {
            CreateProductList();
            using (var context = new P3Referential(options))
            {
                foreach (var p in _testProductsList)
                {
                    context.Product.Add(p);
                }
                context.SaveChanges();
            }
        }

        [Fact]
        private void SaveOrder_GoodPath()
        {
            _orderService.SaveOrder(_orderViewModel);
            Assert.Equal("OrderServiceName", _context.Order.First().Name);
        }
        [Fact]
        private void SaveOrder_BadPath()
        {
            _orderService.SaveOrder(null);
            Assert.Empty(_context.Order);
        }
        [Fact]
        public async void GetOrder_GoodPath()
        {
            _orderService.SaveOrder(_orderViewModel);
            var getOrder = await _orderService.GetOrder(1);
            Assert.NotNull(getOrder);
            Assert.Equal(_context.Order.First().Name, getOrder.Name);
        }
        [Fact]
        public async void GetOrder_BadPath()
        {
            _orderService.SaveOrder(_orderViewModel);
            var getOrder = await _orderService.GetOrder(999);
            Assert.Null(getOrder);
        }
        [Fact]
        public async void GetOrders_GoodPath()
        {
            _orderService.SaveOrder(_orderViewModel);
            _orderService.SaveOrder(_orderViewModel);
            var getOrders = await _orderService.GetOrders();
            Assert.NotNull(getOrders);
            Assert.Equal(2,getOrders.Count);
        }
    }
}
