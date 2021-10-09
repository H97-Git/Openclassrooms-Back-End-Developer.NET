using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using P3AddNewFunctionalityDotNetCore.Controllers;
using P3AddNewFunctionalityDotNetCore.Data;
using P3AddNewFunctionalityDotNetCore.Models.Entities;
using P3AddNewFunctionalityDotNetCore.Models.Repositories;
using P3AddNewFunctionalityDotNetCore.Models.Services;
using P3AddNewFunctionalityDotNetCore.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace P3AddNewFunctionalityDotNetCore.Tests.Controllers
{
    public class ProductControllerTests
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
        public void Index_GoodPath()
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
        public void Admin_GoodPath()
        {
            var options = DbContextOptionsBuilder();
            SeedTestDb(options);
            using (var context = new P3Referential(options))
            {
                ProductRepository productRepository = new ProductRepository(context);
                ProductService productService = new ProductService(null, productRepository, null, null);
                var controller = new ProductController(productService, null);

                ViewResult resultAdmin = controller.Admin() as ViewResult;

                Assert.NotNull(resultAdmin);
                var model = resultAdmin.Model  as IEnumerable<ProductViewModel>;
                Assert.NotNull(model);
                Assert.Equal(3, model.Count());
            }
        }
        [Fact]
        public void Create_ViewResult_GoodPath()
        {
            var options = DbContextOptionsBuilder();
            SeedTestDb(options);
            using (var context = new P3Referential(options))
            {
                ProductRepository productRepository = new ProductRepository(context);
                ProductService productService = new ProductService(null, productRepository, null, null);
                var controller = new ProductController(productService, null);
                ViewResult resultCreate = controller.Create();
                Assert.Null(resultCreate.ViewName);
            }
        }
        [Fact]
        public void Create_IActionResult_GoodPath()
        {
            var options = DbContextOptionsBuilder();
            SeedTestDb(options);
            using (var context = new P3Referential(options))
            {
                ProductRepository productRepository = new ProductRepository(context);
                ProductService productService = new ProductService(null, productRepository, null, null);
                ProductViewModel productViewModel = new ProductViewModel
                {
                    Description = "Test",
                    Details = "Tests",
                    Name = "TEST",
                    Price = "10",
                    Stock = "1",
                };
                var controller = new ProductController(productService, null);
                var beforeProductsS = productService.GetAllProducts();
                var beforeProductsR = productRepository.GetAllProducts().ToList();
                controller.Create(productViewModel);
                var afterProductsS = productService.GetAllProducts();
                var afterProductsR = productRepository.GetAllProducts().ToList();
                Assert.NotNull(controller);
                Assert.Equal(beforeProductsS.Count() + 1, afterProductsS.Count);
                Assert.Equal(beforeProductsR.Count() + 1, afterProductsR.Count);
            }
        }
        [Fact]
        public void DeleteProduct_GoodPath()
        {
            var options = DbContextOptionsBuilder();
            SeedTestDb(options);
            using (var context = new P3Referential(options))
            {
                ProductRepository productRepository = new ProductRepository(context);
                ProductService productService = new ProductService(null, productRepository, null, null);
                var controller = new ProductController(productService, null);
                var beforeProductsS = productService.GetAllProducts();
                var beforeProductsR = productRepository.GetAllProducts().ToList();
                controller.DeleteProduct(1);
                var afterProductsS = productService.GetAllProducts();
                var afterProductsR = productRepository.GetAllProducts().ToList();
                Assert.NotNull(controller);
                Assert.Equal(beforeProductsS.Count() - 1, afterProductsS.Count);
                Assert.Equal(beforeProductsR.Count() - 1, afterProductsR.Count);
            }
        }
    }
    
}
