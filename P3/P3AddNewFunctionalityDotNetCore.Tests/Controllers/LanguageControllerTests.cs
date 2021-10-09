using Microsoft.AspNetCore.Http;
using P3AddNewFunctionalityDotNetCore.Controllers;
using P3AddNewFunctionalityDotNetCore.Models.Services;
using P3AddNewFunctionalityDotNetCore.Models.ViewModels;
using System.Linq;
using Xunit;

namespace P3AddNewFunctionalityDotNetCore.Tests.Controllers
{
    public class LanguageControllerTests
    {
        [Fact]
        public void ChangeUiLanguage_GoodPath()
        {
            // Thanks : https://stackoverflow.com/questions/43429568/unit-testing-net-core-web-api-controllers-with-jwt-claims/43429756

            HttpContext httpContext = new DefaultHttpContext();
            var languageService = new LanguageService();
            var languageController = new LanguageController(languageService)
            {
                ControllerContext = { HttpContext = httpContext }
            };
            var languageViewModel = new LanguageViewModel
            {
                Language = "English"
            };

            languageController.ChangeUiLanguage(languageViewModel, "/");
            var cookies = httpContext.Response.Headers.Values.FirstOrDefault().ToList();
            var cookie = cookies.First();

            Assert.NotNull(cookies);
            Assert.Equal(".AspNetCore.Culture=c%3Den%7Cuic%3Den; path=/", cookie);
        }

        [Fact]
        public void ChangeUiLanguage_BadPath()
        {
            // Thanks : https://stackoverflow.com/questions/43429568/unit-testing-net-core-web-api-controllers-with-jwt-claims/43429756

            HttpContext httpContext = new DefaultHttpContext();
            var languageService = new LanguageService();

            var languageController = new LanguageController(languageService)
            {
                ControllerContext = { HttpContext = httpContext }
            };
            var languageViewModel = new LanguageViewModel
            {
                Language = null
            };

            languageController.ChangeUiLanguage(languageViewModel, "/");
            var cookies = httpContext.Response.Headers.Values.FirstOrDefault().ToList();

            Assert.Empty(cookies);
        }
    }
}
