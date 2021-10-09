using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using P3AddNewFunctionalityDotNetCore.Data;
using P3AddNewFunctionalityDotNetCore.Models;
using P3AddNewFunctionalityDotNetCore.Models.Entities;
using P3AddNewFunctionalityDotNetCore.Models.Repositories;
using Xunit;
using Xunit.Abstractions;

namespace P3AddNewFunctionalityDotNetCore.Tests
{
    public class CartTests
    {
        private readonly ITestOutputHelper _testOutputHelper;
        public CartTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }
        [Fact]
        public void Lines_GoodPath()
        {
            Cart cart = new Cart();
            Product product = new Product
            {
                Name = "one name",
                Description = "one description",
                Details = "one details",
                Quantity = 6,
                Price = 30
            };
            cart.AddItem(product, 1);
            var result = cart.Lines.FirstOrDefault()?.Product.Name;
            Assert.NotNull(cart);
            Assert.NotNull(result);
            Assert.Equal("one name", result);
        }
        [Fact]
        public void AddItem_GoodPath()
        {
            Cart cart = new Cart();
            Product product = new Product
            {
                Name = "one name",
                Description = "one description",
                Details = "one details",
                Quantity = 6,
                Price = 30
            };
            cart.AddItem(product, 1);
            var result = cart.Lines.FirstOrDefault()?.Product.Name;
            Assert.NotNull(cart);
            Assert.NotNull(result);
            Assert.Equal("one name", result);
        }
        [Fact]
        public void AddItem_BadPath_ProductNull()
        {
            Cart cart = new Cart();
            Product product = null;
            cart.AddItem(product, 1);
            Assert.Empty(cart.Lines);
        }
        [Fact]
        public void AddItem_BadPath_QuantityMoreThanStock()
        {
            Cart cart = new Cart();
            Product product = new Product
            {
                Name = "one name",
                Description = "one description",
                Details = "one details",
                Quantity = 6,
                Price = 30
            };
            cart.AddItem(product, 7);
            Assert.Empty(cart.Lines);
        }
        [Fact]
        public void RemoveLine_GoodPath_OneItem()
        {
            Cart cart = new Cart();
            Product product = new Product
            {
                Name = "one name",
                Description = "one description",
                Details = "one details",
                Quantity = 6,
                Price = 30
            };
            cart.AddItem(product, 1);
            cart.RemoveLine(product);
            var result = cart.Lines;
            Assert.NotNull(cart);
            Assert.NotNull(result);
            Assert.Empty(result);
        }
        [Fact]
        public void RemoveLine_GoodPath_ProductNull()
        {
            Cart cart = new Cart();
            cart.AddItem(null, 1);
            cart.RemoveLine(null);
            var result = cart.Lines;
            Assert.NotNull(cart);
            Assert.NotNull(result);
            Assert.Empty(result);
        }
        [Fact]
        public void GetTotalValue_GoodPath_OneItem()
        {
            Cart cart = new Cart();
            Product product = new Product
            {
                Name = "one name",
                Description = "one description",
                Details = "one details",
                Quantity = 6,
                Price = 30
            };
            cart.AddItem(product, 3);
            var result = cart.GetTotalValue();
            Assert.NotNull(cart);
            Assert.Equal(90,result);
        }
        [Fact]
        public void GetTotalValue_GoodPath_MultiItem()
        {
            Cart cart = new Cart();
            Product productOne = new Product
            {
                Name = "one name",
                Description = "one description",
                Details = "one details",
                Quantity = 6,
                Price = 30
            };
            Product productTwo = new Product
            {
                Name = "two name",
                Description = "two description",
                Details = "two details",
                Quantity = 5,
                Price = 30
            };
            Product productThree = new Product
            {
                Name = "three name",
                Description = "three description",
                Details = "three details",
                Quantity = 4,
                Price = 30
            };
            cart.AddItem(productOne, 1);

            cart.AddItem(productTwo, 1);

            cart.AddItem(productThree, 1);

            cart.AddItem(productOne, 1);

            var result = cart.GetTotalValue();
            Assert.NotNull(cart);
            Assert.Equal(120, result);
        }
        [Fact]
        public void GetTotalValue_GoodPath_CartEmpty()
        {
            Cart cart = new Cart();
            var result = cart.GetTotalValue();
            Assert.Equal(0, result);
        }
        [Fact]
        public void GetTotalValue_BadPath_ProductNull()
        {
            Cart cart = new Cart();
            Product product = null;
            cart.AddItem(product, 3);
            var result = cart.GetTotalValue();
            Assert.Equal(0,result);
        }
        [Fact]
        public void GetAverageValue_GoodPath() {
            Cart cart = new Cart();
            Product product = new Product
            {
                Name = "one name",
                Description = "one description",
                Details = "one details",
                Quantity = 6,
                Price = 30
            };
            cart.AddItem(product, 3);
            var result = cart.GetAverageValue();
            Assert.NotNull(cart);
            Assert.Equal(30, result);
        }
        [Fact]
        public void GetCartLine_GoodPath_OneItem()
        {
            Cart cart = new Cart();
            Product product = new Product
            {
                Name = "one name",
                Description = "one description",
                Details = "one details",
                Quantity = 6,
                Price = 30
            };
            cart.AddItem(product, 4);
            var result = cart.GetCartLine();
            Assert.NotNull(cart);
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(4, result.FirstOrDefault().Quantity);
        }
        [Fact]
        public void GetCartLine_GoodPath_EmptyCart()
        {
            Cart cart = new Cart();
            var result = cart.GetCartLine();
            Assert.NotNull(cart);
            Assert.Empty(result);
        }
        [Fact]
        public void Clear_GoodPath_OneItem()
        {
            Cart cart = new Cart();
            Product product = new Product
            {
                Name = "one name",
                Description = "one description",
                Details = "one details",
                Quantity = 6,
                Price = 30
            };
            cart.AddItem(product, 4);
            cart.Clear();
            var result = cart.GetCartLine();
            Assert.NotNull(cart);
            Assert.NotNull(result);
            Assert.Empty(result);
        }
        [Fact]
        public void Clear_GoodPath_EmptyCart()
        {
            Cart cart = new Cart();
            cart.Clear();
            var result = cart.GetCartLine();
            Assert.NotNull(cart);
            Assert.Empty(result);
        }
    }
}
