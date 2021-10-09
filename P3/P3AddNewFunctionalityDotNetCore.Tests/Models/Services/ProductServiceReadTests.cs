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
using Xunit;
using Xunit.Abstractions;

namespace P3AddNewFunctionalityDotNetCore.Tests.Models.Services
{
    [Collection("Sequential")]
    public class ProductServiceReadTests
    {
        private readonly ITestOutputHelper _testOutputHelper;
        private readonly DbContextOptions<P3Referential> _options;
        private string connectionString =
            "Server=.\\SQLEXPRESS;Database=P3Referential-2f561d3b-493f-46fd-83c9-6e2643e7bd0a;Trusted_Connection=True;MultipleActiveResultSets=true";


        public ProductServiceReadTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
            // Test for real database
            _options = new DbContextOptionsBuilder<P3Referential>().UseSqlServer(connectionString)
                .Options;
        }

        public IEnumerable<Product> _testProductsList;

        [Fact]
        public void GetAllProducts_GoodPath()
        {
            using (var context = new P3Referential(_options))
            {
                var productRepository = new ProductRepository(context);
                var productService = new ProductService(null,productRepository,null,null);
                var getAllProducts = productService.GetAllProducts();
                Assert.NotNull(getAllProducts);
            }
        }

        [Fact]
        public void GetProductById_GoodPath()
        {
            using (var context = new P3Referential(_options))
            {
                var productRepository = new ProductRepository(context);
                var productService = new ProductService(null, productRepository, null, null);
                var getProductById = productService.GetProductById(1);
                Assert.NotNull(getProductById);
                Assert.Equal("Echo Dot", getProductById.Name);
            }
        }
        [Fact]
        public void GetProductById_BadPath()
        {
            using (var context = new P3Referential(_options))
            {
                var productRepository = new ProductRepository(context);
                var productService = new ProductService(null, productRepository, null, null);
                var getProductById = productService.GetProductById(999);
                Assert.Null(getProductById);
            }
        }
        [Fact]
        public async void GetProductByIdAsync_GoodPath()
        {
            using (var context = new P3Referential(_options))
            {
                var productRepository = new ProductRepository(context);
                var productService = new ProductService(null, productRepository, null, null);
                var getProductByIdAsync = await productService.GetProduct(1);
                Assert.NotNull(getProductByIdAsync);
                Assert.Equal("Echo Dot", getProductByIdAsync.Name);
            }
        }

        [Fact]
        public async void GetProductByIdAsync_BadPath()
        {
            using (var context = new P3Referential(_options))
            {
                var productRepository = new ProductRepository(context);
                var productService = new ProductService(null, productRepository, null, null);
                var getProductByIdAsync = await productService.GetProduct(999);
                Assert.Null(getProductByIdAsync);
            }
        }
        [Fact]
        public async void GetAllProductAsync_GoodPath()
        {
            using (var context = new P3Referential(_options))
            {
                var productRepository = new ProductRepository(context);
                var productService = new ProductService(null, productRepository, null, null);
                var getAllProductAsync = await productService.GetProduct();
                Assert.NotNull(getAllProductAsync);
                Assert.Equal(5, getAllProductAsync.Count);
            }
        }

        [Fact]
        public void GetAllProductsViewModel_GoodPath()
        {
            using (var context = new P3Referential(_options))
            {
                var productRepository = new ProductRepository(context);
                var productService = new ProductService(null, productRepository, null, null);
                var getAllProductsViewModel = productService.GetAllProductsViewModel();
                Assert.NotNull(getAllProductsViewModel);
                Assert.Equal(5, getAllProductsViewModel.Count);
            }
        }

        [Fact]
        public void GetProductByIdViewModel_GoodPath()
        {
            using (var context = new P3Referential(_options))
            {
                var productRepository = new ProductRepository(context);
                var productService = new ProductService(null, productRepository, null, null);
                var getProductByIdViewModel = productService.GetProductByIdViewModel(1);
                Assert.NotNull(getProductByIdViewModel);
                Assert.Equal("Echo Dot", getProductByIdViewModel.Name);
            }
        }

        [Fact]
        public void GetProductByIdViewModel_BadPath()
        {
            using (var context = new P3Referential(_options))
            {
                var productRepository = new ProductRepository(context);
                var productService = new ProductService(null, productRepository, null, null);
                var getProductByIdViewModel = productService.GetProductByIdViewModel(999);
                Assert.Null(getProductByIdViewModel);
            }
        }

    }
}
