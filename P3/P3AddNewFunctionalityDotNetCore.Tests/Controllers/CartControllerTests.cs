using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Moq;
using P3AddNewFunctionalityDotNetCore.Controllers;
using P3AddNewFunctionalityDotNetCore.Data;
using P3AddNewFunctionalityDotNetCore.Models;
using P3AddNewFunctionalityDotNetCore.Models.Entities;
using P3AddNewFunctionalityDotNetCore.Models.Repositories;
using P3AddNewFunctionalityDotNetCore.Models.Services;
using Xunit;

namespace P3AddNewFunctionalityDotNetCore.Tests.Controllers
{
    public class CartControllerTests
    {
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
        public void Index_GoodPath()
        {
            var options = DbContextOptionsBuilder();
            SeedTestDb(options);
            using (var context = new P3Referential(options))
            {
                var productRepository = new ProductRepository(context);
                var cart = new Cart();
                var productService = new ProductService(cart, productRepository, null, null);
                cart.AddItem(productRepository.GetAllProducts().First(),1);
                cart.AddItem(productRepository.GetAllProducts().Last(), 1);
                var controller = new CartController(cart,productService);

                ViewResult resultIndex = controller.Index() as ViewResult;
                var model = resultIndex.Model as Cart;

                Assert.NotNull(model);
                Assert.Equal(2, model.Lines.Count());
            }
        }

        [Fact]
        public void AddToCart_GoodPath()
        {
            var options = DbContextOptionsBuilder();
            SeedTestDb(options);
            using (var context = new P3Referential(options))
            {
                var productRepository = new ProductRepository(context);
                var cart = new Cart();
                var productService = new ProductService(cart, productRepository, null, null);
                
                var controller = new CartController(cart, productService);

                controller.AddToCart(1);

                Assert.Single(cart.Lines);

            }
        }
        [Fact]
        public void AddToCart_BadPath()
        {
            var options = DbContextOptionsBuilder();
            SeedTestDb(options);
            using (var context = new P3Referential(options))
            {
                var productRepository = new ProductRepository(context);
                var cart = new Cart();
                var productService = new ProductService(cart, productRepository, null, null);

                var controller = new CartController(cart, productService);

                controller.AddToCart(999);

                Assert.Empty(cart.Lines);

            }
        }
        [Fact]
        public void RemoveFromCart_GoodPath()
        {
            var options = DbContextOptionsBuilder();
            SeedTestDb(options);
            using (var context = new P3Referential(options))
            {
                var productRepository = new ProductRepository(context);
                var cart = new Cart();
                var productService = new ProductService(cart, productRepository, null, null);

                var controller = new CartController(cart, productService);

                controller.AddToCart(1);
                controller.RemoveFromCart(1);

                Assert.Empty( cart.Lines);

            }
        }
    }
}
