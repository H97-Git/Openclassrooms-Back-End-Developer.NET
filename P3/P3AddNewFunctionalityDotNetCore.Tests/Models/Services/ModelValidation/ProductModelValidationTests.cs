using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Moq;
using P3AddNewFunctionalityDotNetCore.Models.Entities;
using P3AddNewFunctionalityDotNetCore.Models.Repositories;
using P3AddNewFunctionalityDotNetCore.Models.Services;
using P3AddNewFunctionalityDotNetCore.Models.ViewModels;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace P3AddNewFunctionalityDotNetCore.Tests
{


    public class ProductModelValidationTests
    {
        private readonly Mock<IStringLocalizer<ProductService>> _fakeLocalizer;

        public ProductModelValidationTests()
        {
            _fakeLocalizer = new Mock<IStringLocalizer<ProductService>>();
        }

        #region Name Validation

        public ProductViewModel CreateProductByName(string input)
        {
            var product = new ProductViewModel
            {
                Id = 1,
                Description = "one",
                Details = "details",
                Name = input,
                Price = "1",
                Stock = "1"
            };
            return product;
        }

        [Theory]
        [InlineData("", "MissingName", true)] // Empty Name return MissingName error message
        [InlineData("    ", "MissingName", true)] // White Space Name return MissingName error message
        [InlineData("ab", "NameNotInRange", true)] // Name less than three characters return NotInRange error message
        [InlineData("01234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567891", "NameNotInRange", true)] // Name more than a hundred characters
        [InlineData(" Test ", "", false)] // Name start and end with whitespace return empty
        [InlineData("     Test2    ", "", false)] // Name start and end with whitespaces return empty
        public void CheckProductModelErrors_NameValidation_BadPath(string input, string expected, bool validationNotNull)
        {

            //Arrange
            var localizedString = new LocalizedString(expected, expected);
            _fakeLocalizer.Setup(_ => _[expected]).Returns(localizedString);
            var product = CreateProductByName(input);
            var productService = new ProductService(null, null, null, _fakeLocalizer.Object);
            // Act
            var result = productService.CheckProductModelErrors(product);
            //Assert
            if (validationNotNull)
            {
                Assert.NotNull(result);
                Assert.Equal(result[0], expected);
            }
            else
            {
                Assert.Empty(result);
            }
        }

        [Theory]
        [InlineData("Test", "")]// Testing a good name
        [InlineData("Test123", "")] // Testing a second good name 
        public void CheckProductModelErrors_NameValidation_GoodPath(string input, string expected)
        {
            //Arrange
            var localizedString = new LocalizedString(expected, expected);
            _fakeLocalizer.Setup(_ => _[expected]).Returns(localizedString);
            var product = CreateProductByName(input);
            var productService = new ProductService(null, null, null, _fakeLocalizer.Object);
            // Act
            var result = productService.CheckProductModelErrors(product);
            //Assert
            Assert.Empty(result);
        }

        #endregion

        #region Description Validation

        public ProductViewModel CreateProductByDescription(string input)
        {
            var product = new ProductViewModel
            {
                Id = 1,
                Description = input,
                Details = "details",
                Name = "Name",
                Price = "1",
                Stock = "1"
            };
            return product;
        }

        [Theory]
        [InlineData("", "MissingDescription", true)] // Empty Description return MissingDescription error message
        [InlineData("    ", "MissingDescription", true)] // White Space Description return MissingDescription error message
        [InlineData("ab", "DescriptionNotInRange", true)] // Description less than three characters return NotInRange error message
        [InlineData("108925821147171293588284403483740442404347379242849211976415417701740900520640895640649173964848817063532811861423302337333437047812863027777526120892767110632703999545424360674491187797905912117528773635615910092713039833197129021898048217994637995694153845975004414075804863736354831951304201999463968109522651972828209227999724339722097715165675773812105055249035499566271692685032103877357735375249497046206379913768726391156546756859350342757799752641313097592768865723534323108567537878942555349", "DescriptionNotInRange", true)] // Description more than five hundred characters
        [InlineData(" Test ", "", false)] // Description start and end with whitespace return empty
        [InlineData("     Test2    ", "", false)] // Description start and end with whitespaces return empty
        public void CheckProductModelErrors_DescriptionValidation_BadPath(string input, string expected, bool validationNotNull)
        {

            //Arrange
            var localizedString = new LocalizedString(expected, expected);
            _fakeLocalizer.Setup(_ => _[expected]).Returns(localizedString);
            var product = CreateProductByDescription(input);
            var productService = new ProductService(null, null, null, _fakeLocalizer.Object);
            // Act
            var result = productService.CheckProductModelErrors(product);
            //Assert
            if (validationNotNull)
            {
                Assert.NotNull(result);
                Assert.Equal(result[0], expected);
            }
            else
            {
                Assert.Empty(result);
            }
        }

        [Theory]
        [InlineData("Test", "")]// Testing a good Description
        [InlineData("Test123", "")] // Testing a second good Description 
        public void CheckProductModelErrors_DescriptionValidation_GoodPath(string input, string expected)
        {
            //Arrange
            var localizedString = new LocalizedString(expected, expected);
            _fakeLocalizer.Setup(_ => _[expected]).Returns(localizedString);
            var product = CreateProductByDescription(input);
            var productService = new ProductService(null, null, null, _fakeLocalizer.Object);
            // Act
            var result = productService.CheckProductModelErrors(product);
            //Assert
            Assert.Empty(result);
        }
        #endregion

        #region Stock Validation
        public ProductViewModel CreateProductByStock(string input)
        {
            var product = new ProductViewModel
            {
                Id = 1,
                Description = "description",
                Details = "details",
                Name = "Name",
                Price = "1",
                Stock = input
            };
            return product;
        }
        [Theory]
        [InlineData("", "MissingQuantity", true)] 
        [InlineData("    ", "MissingQuantity", true)] 
        [InlineData("abc", "StockNotAnInteger", true)] 
        [InlineData("-1", "StockNotGreaterThanZero", true)] 
        [InlineData("1000", "StockGreaterThanMaxStock", true)] 
        [InlineData(" 123 ", "", false)]
        [InlineData("    10    ", "", false)]
        public void CheckProductModelErrors_StockValidation_BadPath(string input, string expected, bool validationNotNull)
        {

            //Arrange
            var localizedString = new LocalizedString(expected, expected);
            _fakeLocalizer.Setup(_ => _[expected]).Returns(localizedString);
            var product = CreateProductByStock(input);
            var productService = new ProductService(null, null, null, _fakeLocalizer.Object);
            // Act
            var result = productService.CheckProductModelErrors(product);
            //Assert
            if (validationNotNull)
            {
                Assert.NotNull(result);
                Assert.Equal(result[0], expected);
            }
            else
            {
                Assert.Empty(result);
            }
        }

        [Theory]
        [InlineData("1", "")]
        [InlineData("10", "")]
        [InlineData("100", "")] 
        public void CheckProductModelErrors_StockValidation_GoodPath(string input, string expected)
        {
            //Arrange
            var localizedString = new LocalizedString(expected, expected);
            _fakeLocalizer.Setup(_ => _[expected]).Returns(localizedString);
            var product = CreateProductByStock(input);
            var productService = new ProductService(null, null, null, _fakeLocalizer.Object);
            // Act
            var result = productService.CheckProductModelErrors(product);
            //Assert
            Assert.Empty(result);
        }
        #endregion

        #region Price Validation
        public ProductViewModel CreateProductByPrice(string input)
        {
            var product = new ProductViewModel
            {
                Id = 1,
                Description = "description",
                Details = "details",
                Name = "Name",
                Price = input,
                Stock = "10"
            };
            return product;
        }
        [Theory]
        [InlineData("", "MissingPrice", true)]
        [InlineData("    ", "MissingPrice", true)]
        [InlineData("abc", "PriceNotANumber", true)]
        [InlineData("-1", "PriceNotGreaterThanZero", true)]
        [InlineData("1000", "PriceGreaterThanMaxPrice", true)]
        [InlineData(" 123 ", "", false)]
        [InlineData("    10    ", "", false)]
        public void CheckProductModelErrors_PriceValidation_BadPath(string input, string expected, bool validationNotNull)
        {

            //Arrange
            var localizedString = new LocalizedString(expected, expected);
            _fakeLocalizer.Setup(_ => _[expected]).Returns(localizedString);
            var product = CreateProductByPrice(input);
            var productService = new ProductService(null, null, null, _fakeLocalizer.Object);
            // Act
            var result = productService.CheckProductModelErrors(product);
            //Assert
            if (validationNotNull)
            {
                Assert.NotNull(result);
                Assert.Equal(result[0], expected);
            }
            else
            {
                Assert.Empty(result);
            }
        }

        [Theory]
        [InlineData("1", "")]
        [InlineData("10", "")]
        [InlineData("100", "")]
        public void CheckProductModelErrors_PriceValidation_GoodPath(string input, string expected)
        {
            //Arrange
            var localizedString = new LocalizedString(expected, expected);
            _fakeLocalizer.Setup(_ => _[expected]).Returns(localizedString);
            var product = CreateProductByPrice(input);
            var productService = new ProductService(null, null, null, _fakeLocalizer.Object);
            // Act
            var result = productService.CheckProductModelErrors(product);
            //Assert
            Assert.Empty(result);
        }


        #endregion
        
    }
}
