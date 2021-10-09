using System;
using Microsoft.EntityFrameworkCore;
using P3AddNewFunctionalityDotNetCore.Data;
using P3AddNewFunctionalityDotNetCore.Models.Entities;
using P3AddNewFunctionalityDotNetCore.Models.Repositories;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace P3AddNewFunctionalityDotNetCore.Tests.Models.Repositories
{
    [Collection("Sequential")]
    public class ProductRepositoryReadTests
    {
        private readonly DbContextOptions<P3Referential> _options;
        private string connectionString =
            "Server=.\\SQLEXPRESS;Database=P3Referential-2f561d3b-493f-46fd-83c9-6e2643e7bd0a;Trusted_Connection=True;MultipleActiveResultSets=true";

        public ProductRepositoryReadTests()
        {
            // Test for real database
            _options = new DbContextOptionsBuilder<P3Referential>().UseSqlServer(connectionString)
                .Options;
        }

        [Fact]
        public async void GetProductById_GoodPath(){
            
            using (var context = new P3Referential(_options))
            {
                var productRepository = new ProductRepository(context);
                var getProduct = await productRepository.GetProduct(1);
                Assert.NotNull(getProduct);
                Assert.Equal("Echo Dot" , getProduct.Name);
            }
        }
        [Theory]
        [InlineData(999)]
        [InlineData(0)]
        public async void GetProductById_BadPath(int id)
        {
            using (var context = new P3Referential(_options))
            {
                var productRepository = new ProductRepository(context);
                var getProduct = await productRepository.GetProduct(id);
                Assert.Null(getProduct);
            }
        }
        // Test : Get all product async
        [Fact]
        public async void GetAllProductAsync_GoodPath()
        {
            using (var context = new P3Referential(_options))
            {
                var productRepository = new ProductRepository(context);
                var getProductAsync = await productRepository.GetProduct();
                Assert.NotNull(getProductAsync);
                Assert.NotEmpty(getProductAsync);
                Assert.Equal(5, getProductAsync.Count);
            }
        }
        // Test : Get all product
        [Fact]
        public void GetAllProduct_GoodPath()
        {
            using (var context = new P3Referential(_options))
            {
                var productRepository = new ProductRepository(context);
                var getAllProduct = productRepository.GetAllProducts().ToList();
                Assert.NotNull(getAllProduct);
                Assert.NotEmpty(getAllProduct);
                Assert.Equal(5, getAllProduct.Count());
            }
        }
    }
}
