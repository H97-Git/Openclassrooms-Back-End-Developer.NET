using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using P3AddNewFunctionalityDotNetCore.Controllers;
using P3AddNewFunctionalityDotNetCore.Data;
using P3AddNewFunctionalityDotNetCore.Models;
using P3AddNewFunctionalityDotNetCore.Models.Entities;
using P3AddNewFunctionalityDotNetCore.Models.Repositories;
using P3AddNewFunctionalityDotNetCore.Models.Services;
using P3AddNewFunctionalityDotNetCore.Models.ViewModels;
using Xunit;

namespace P3AddNewFunctionalityDotNetCore.Tests.Controllers
{
    public class OrderControllerTests
    {
        private readonly OrderService _orderService;
        private readonly OrderViewModel _orderViewModel;

        public OrderControllerTests()
        {
            var options = DbContextOptionsBuilder();
            var context = new P3Referential(options);
            var productRepository = new ProductRepository(context);
            var orderRepository = new OrderRepository(context);
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
        public void Index_ViewResult_GoodPath()
        {
            var options = DbContextOptionsBuilder();
            SeedTestDb(options);
            using (var context = new P3Referential(options))
            {
                ProductRepository productRepository = new ProductRepository(context);
                ProductService productService = new ProductService(null, productRepository, null, null);

                var controller = new ProductController(productService, null);

                ViewResult resultIndex = controller.Index() as ViewResult;

                Assert.NotNull(resultIndex);
                var model = resultIndex.Model as List<ProductViewModel>;

                Assert.NotNull(model);
                Assert.Equal(3, model.Count);
            }
        }
        [Fact]
        public async void Index_IActionResult_GoodPath()
        {
            var options = DbContextOptionsBuilder();
            SeedTestDb(options);
            using (var context = new P3Referential(options))
            {
                ProductRepository productRepository = new ProductRepository(context);
                OrderRepository orderRepository = new OrderRepository(context);

                Cart cart = new Cart();
                Product product = await productRepository.GetProduct(1);
                cart.AddItem(product, 1);

                ProductService productService = new ProductService(cart, productRepository, null, null);
                OrderService orderService = new OrderService(cart,orderRepository,productService);

                OrderViewModel orderViewModel = new OrderViewModel
                {
                    Address = "Address",
                    City = "City",
                    Country = "Country",
                    Date = DateTime.Now,
                    Name = "OrderServiceName",
                    Zip = "00000",
                    Lines = ((Cart)cart)?.Lines.ToArray(),
                };

                var controller = new OrderController(cart,orderService,null);
                controller.Index(orderViewModel);
                Assert.Equal(1,context.Order.Count());
            }
        }
        [Fact]
        public async void Completed_GoodPath()
        {
            var options = DbContextOptionsBuilder();
            SeedTestDb(options);
            using (var context = new P3Referential(options))
            {
                ProductRepository productRepository = new ProductRepository(context);
                OrderRepository orderRepository = new OrderRepository(context);

                Cart cart = new Cart();
                Product product = await productRepository.GetProduct(1);
                cart.AddItem(product, 1);

                ProductService productService = new ProductService(cart, productRepository, null, null);
                OrderService orderService = new OrderService(cart, orderRepository, productService);


                var controller = new OrderController(cart, orderService, null);
                var viewResult = controller.Completed() as ViewResult;
                Assert.Empty(cart.Lines);
                Assert.Null(viewResult.ViewName);
            }
        }
    }
}
