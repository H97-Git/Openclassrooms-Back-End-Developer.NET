using System;
using Microsoft.EntityFrameworkCore;
using P3AddNewFunctionalityDotNetCore.Data;
using P3AddNewFunctionalityDotNetCore.Models.Entities;
using P3AddNewFunctionalityDotNetCore.Models.Repositories;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Microsoft.EntityFrameworkCore.Storage;
using P3AddNewFunctionalityDotNetCore.Models;
using P3AddNewFunctionalityDotNetCore.Models.Services;
using Xunit;
using Xunit.Abstractions;

namespace P3AddNewFunctionalityDotNetCore.Tests.Models.Repositories
{
    [Collection("Sequential")]
    public class ProductRepositoryWriteTests
    {
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
        public async void UpdateProduct_GoodPath()
        {
            var options = DbContextOptionsBuilder();
            SeedTestDb(options);
            using (var context = new P3Referential(options))
            {
                var productRepository = new ProductRepository(context);
                productRepository.UpdateProductStocks(1,1);
                var getProduct = await productRepository.GetProduct(1);
                Assert.NotNull(getProduct);
                Assert.Equal( 1, getProduct.Quantity);
                context.Database.EnsureDeleted();
            }
        }
        [Theory]
        [InlineData(1,3)] // Product.Quantity == 3; id = 1;
        [InlineData(1,2)] //Product.Quantity == 2; id = 1;
        public async void UpdateProduct_Remove_GoodPath(int id, int quantityToRemove)
        {
            var options = DbContextOptionsBuilder();
            SeedTestDb(options);
            using (var context = new P3Referential(options))
            {
                var productRepository = new ProductRepository(context);
                productRepository.UpdateProductStocks(id, quantityToRemove);
                var allProduct = productRepository.GetAllProducts();
                var getProduct = await productRepository.GetProduct(id);
                Assert.Null(getProduct);
                Assert.Equal(2 , allProduct.Count());
                context.Database.EnsureDeleted();
            }
        }
        [Theory]
        [InlineData(1, -1)] // Product.Quantity == -1;
        [InlineData(1, 0)] // Product.Quantity == 0;
        public async void UpdateProduct_Path(int id, int quantityToRemove)
        {
            var options = DbContextOptionsBuilder();
            SeedTestDb(options);
            using (var context = new P3Referential(options))
            {
                var productRepository = new ProductRepository(context);
                productRepository.UpdateProductStocks(id, quantityToRemove);
                var allProduct = productRepository.GetAllProducts();
                var getProduct = await productRepository.GetProduct(id);
                Assert.NotNull(getProduct);
                Assert.Equal( 3, allProduct.Count());
                context.Database.EnsureDeleted();
            }
        }
        [Theory]
        [InlineData(999, 999)] // id = 999;
        [InlineData(-1, 0)] // id = -1;
        public async void UpdateProduct_Remove_BadPath(int id, int quantityToRemove)
        {
            var options = DbContextOptionsBuilder();
            SeedTestDb(options);
            using (var context = new P3Referential(options))
            {
                var productRepository = new ProductRepository(context);
                productRepository.UpdateProductStocks(id, quantityToRemove);
                var allProduct = productRepository.GetAllProducts();
                var getProduct = await productRepository.GetProduct(id);
                Assert.Null(getProduct);
                Assert.Equal(3, allProduct.Count());
                context.Database.EnsureDeleted();
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
                var product = new Product
                {
                    Name = "four name",
                    Description = "four description",
                    Details = "four details",
                    Quantity = 6,
                    Price = 30
                };
                productRepository.SaveProduct(product);
                var getAllProduct = await productRepository.GetProduct();
                Assert.Equal(4, getAllProduct.Count);
                context.Database.EnsureDeleted();
            }
        }
        [Fact]
        public async void SaveProduct_BadPath() // "BadPath"
        {
            var options = DbContextOptionsBuilder();
            SeedTestDb(options);
            using (var context = new P3Referential(options))
            {
                var productRepository = new ProductRepository(context);
                productRepository.SaveProduct(null);
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
                productRepository.DeleteProduct(1);
                var getAllProduct = await productRepository.GetProduct();
                Assert.Equal(2, getAllProduct.Count);
                context.Database.EnsureDeleted();
            }
        }
        [Fact]
        public async void DeleteProduct_BadPath() // "BadPath"
        {
            var options = DbContextOptionsBuilder();
            SeedTestDb(options);
            using (var context = new P3Referential(options))
            {
                var productRepository = new ProductRepository(context);
                productRepository.DeleteProduct(999);
                var getAllProduct = await productRepository.GetProduct();
                Assert.Equal(3, getAllProduct.Count);
                context.Database.EnsureDeleted();
            }
        }

    }
}
