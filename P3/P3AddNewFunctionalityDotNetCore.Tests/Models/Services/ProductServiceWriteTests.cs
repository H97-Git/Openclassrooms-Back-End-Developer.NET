using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using P3AddNewFunctionalityDotNetCore.Data;
using P3AddNewFunctionalityDotNetCore.Models;
using P3AddNewFunctionalityDotNetCore.Models.Entities;
using P3AddNewFunctionalityDotNetCore.Models.Repositories;
using P3AddNewFunctionalityDotNetCore.Models.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using P3AddNewFunctionalityDotNetCore.Models.ViewModels;
using Xunit;
using Xunit.Abstractions;

namespace P3AddNewFunctionalityDotNetCore.Tests.Models.Services
{
    [Collection("Sequential")]
    public class ProductServiceWriteTests
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public ProductServiceWriteTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        // In Memory Database Test
        private DbContextOptions<P3Referential> DbContextOptionsBuilder()
        {
            return new DbContextOptionsBuilder<P3Referential>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString(), new InMemoryDatabaseRoot())
                .Options;
        }

        private IEnumerable<Product> _testProductsList;

        public List<Product> CreateProductList()
        {
            _testProductsList = new List<Product>
            {
                new Product
                {
                    Id = 1,
                    Name = "one name",
                    Description = "one description",
                    Details = "one details",
                    Quantity = 2,
                    Price = 10
                },
                new Product
                {
                    Id = 2,
                    Name = "two name",
                    Description = "two description",
                    Details = "two details",
                    Quantity = 4,
                    Price = 20
                },
                new Product
                {
                    Id = 3,
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
        public async void SaveProduct_GoodPath()
        {
            var options = DbContextOptionsBuilder();
            SeedTestDb(options);
            using (var context = new P3Referential(options))
            {
                var productRepository = new ProductRepository(context);
                var productService = new ProductService(null,productRepository,null,null);
                var product = new ProductViewModel
                {
                    Id = 4,
                    Name = "four name",
                    Description = "four description",
                    Details = "four details",
                    Stock = "6",
                    Price = "30"
                };
                productService.SaveProduct(product);
                var getAllProduct = await productRepository.GetProduct();
                Assert.Equal(4, getAllProduct.Count);
                context.Database.EnsureDeleted();
            }
        }
        [Fact] //Product = null
        public async void SaveProduct_BadPath()
        {
            var options = DbContextOptionsBuilder();
            SeedTestDb(options);
            using (var context = new P3Referential(options))
            {
                var productRepository = new ProductRepository(context);
                var productService = new ProductService(null, productRepository, null, null);
                productService.SaveProduct(null);
                var getAllProduct = await productRepository.GetProduct();
                Assert.Equal(3, getAllProduct.Count);
                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async void DeleteProduct_GoodPath()
        {
            var options = DbContextOptionsBuilder();
            SeedTestDb(options);
            using (var context = new P3Referential(options))
            {
                var productRepository = new ProductRepository(context);
                var product = await productRepository.GetProduct(1);
                var cart = new Cart();
                cart.AddItem(product, 1);
                var productService = new ProductService(cart, productRepository, null, null);
                productService.DeleteProduct(1);
                var getAllProduct = await productService.GetProduct();
                Assert.Equal(2, getAllProduct.Count);
                Assert.Empty(cart.Lines);
                context.Database.EnsureDeleted();
            }
        }
        [Fact]
        public async void DeleteProduct_BadPath()
        {
            var options = DbContextOptionsBuilder();
            SeedTestDb(options);
            using (var context = new P3Referential(options))
            {
                var productRepository = new ProductRepository(context);
                var productService = new ProductService(null, productRepository, null, null);
                productService.DeleteProduct(999);
                var getAllProduct = await productService.GetProduct();
                Assert.Equal(3, getAllProduct.Count);
                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async void UpdateProductQuantities_GoodPath()
        {
            var options = DbContextOptionsBuilder();
            SeedTestDb(options);
            using (var context = new P3Referential(options))
            {
                var productRepository = new ProductRepository(context);
                var product = await productRepository.GetProduct(1);
                var cart = new Cart();
                cart.AddItem(product, 1);
                var productService = new ProductService(cart, productRepository, null, null);
                productService.UpdateProductQuantities();
                product = await productService.GetProduct(1);
                Assert.NotNull(product);
                Assert.Equal(1,product.Quantity);
                context.Database.EnsureDeleted();
            }
        }

    }
}
