using Microsoft.AspNetCore.Http;
using P3AddNewFunctionalityDotNetCore.Models.Services;
using System.Linq;
using Xunit;

namespace P3AddNewFunctionalityDotNetCore.Tests.Models.Services
{
    public class LanguageServiceTests
    {
        [Theory]
        [InlineData("English")]
        [InlineData("French")]
        [InlineData("Spanish")]
        public void ChangeUiLanguage_GoodPath(string language)
        {
            LanguageService languageService = new LanguageService();
            HttpContext httpContext = new DefaultHttpContext();
            languageService.ChangeUiLanguage(httpContext,language);
            var cookies = httpContext.Response.Headers.Values.FirstOrDefault().ToList();
            var cookie = cookies.First();
            Assert.NotNull(cookies);
            Assert.Single(cookies);
            switch (language)
            {
                case ("English"):
                    Assert.Equal(".AspNetCore.Culture=c%3Den%7Cuic%3Den; path=/", cookie);
                    break;
                case ("French"):
                    Assert.Equal(".AspNetCore.Culture=c%3Dfr%7Cuic%3Dfr; path=/", cookie);
                    break;
                case ("Spanish"):
                    Assert.Equal(".AspNetCore.Culture=c%3Des%7Cuic%3Des; path=/", cookie);
                    break;
                default:
                    Assert.Equal(".AspNetCore.Culture=c%3Den%7Cuic%3Den; path=/", cookie);
                    break;
            }

        }
        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void ChangeUiLanguage_BadPath(string language)
        {
            LanguageService languageService = new LanguageService();
            HttpContext httpContext = new DefaultHttpContext();
            languageService.ChangeUiLanguage(httpContext, language);
            var cookies = httpContext.Response.Headers.Values.FirstOrDefault().ToList();
            var cookie = cookies.First();
            Assert.NotNull(cookies);
            Assert.Single(cookies);
            switch (language)
            {
                case (""):
                    Assert.Equal(".AspNetCore.Culture=c%3Den%7Cuic%3Den; path=/", cookie);
                    break;
                case (null):
                    Assert.Equal(".AspNetCore.Culture=c%3Den%7Cuic%3Den; path=/", cookie);
                    break;
                default:
                    Assert.Equal(".AspNetCore.Culture=c%3Den%7Cuic%3Den; path=/", cookie);
                    break;
            }

        }
        [Theory]
        [InlineData("English")]
        [InlineData("French")]
        [InlineData("Spanish")]
        public void SetCulture_GoodPath(string language)
        {
            LanguageService languageService = new LanguageService();
            var culture = languageService.SetCulture(language);
            Assert.NotNull(culture);
            switch (language)
            {
                case ("English"):
                    Assert.Equal("en",culture);
                    break;
                case ("French"):
                    Assert.Equal("fr", culture);
                    break;
                case ("Spanish"):
                    Assert.Equal("es", culture);
                    break;
                default:
                    Assert.Equal("en", culture);
                    break;
            }
        }
        [Theory]
        [InlineData("German")]
        [InlineData("")]
        [InlineData(null)]
        public void SetCulture_BadPath(string language)
        {
            LanguageService languageService = new LanguageService();
            var culture = languageService.SetCulture(language);
            Assert.NotNull(culture);
            switch (language)
            {
                case ("German"):
                    Assert.Equal("en", culture);
                    break;
                default:
                    Assert.Equal("en", culture);
                    break;
            }
        }
        [Theory]
        [InlineData("en")]
        [InlineData("fr")]
        [InlineData("es")]
        public void UpdateCultureCookie_GoodPath(string language)
        {
            LanguageService languageService = new LanguageService();
            HttpContext httpContext = new DefaultHttpContext();
            languageService.UpdateCultureCookie(httpContext, language);
            var cookies = httpContext.Response.Headers.Values.FirstOrDefault().ToList();
            var cookie = cookies.First();
            Assert.NotNull(cookies);
            Assert.Single(cookies);
            switch (language)
            {
                case ("en"):
                    Assert.Equal(".AspNetCore.Culture=c%3Den%7Cuic%3Den; path=/", cookie);
                    break;
                case ("fr"):
                    Assert.Equal(".AspNetCore.Culture=c%3Dfr%7Cuic%3Dfr; path=/", cookie);
                    break;
                case ("es"):
                    Assert.Equal(".AspNetCore.Culture=c%3Des%7Cuic%3Des; path=/", cookie);
                    break;
                default:
                    Assert.Equal(".AspNetCore.Culture=c%3Den%7Cuic%3Den; path=/", cookie);
                    break;
            }
        }
        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void UpdateCultureCookie_BadPath(string language)
        {
            LanguageService languageService = new LanguageService();
            HttpContext httpContext = new DefaultHttpContext();
            languageService.UpdateCultureCookie(httpContext, language);
            var cookies = httpContext.Response.Headers.Values.FirstOrDefault().ToList();
            Assert.NotNull(cookies);
            Assert.Empty(cookies);
        }
    }
}
