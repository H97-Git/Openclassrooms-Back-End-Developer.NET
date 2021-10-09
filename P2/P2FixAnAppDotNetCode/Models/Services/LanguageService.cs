using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;

namespace P2FixAnAppDotNetCode.Models.Services
{
    /// <summary>
    /// Provides services method to manage the application language
    /// </summary>
    public class LanguageService : ILanguageService
    {
        /// <summary>
        /// Set the UI language
        /// </summary>
        public void ChangeUiLanguage(HttpContext context, string language)
        {
            string culture = SetCulture(language);
            UpdateCultureCookie(context, culture);
        }

        /// <summary>
        /// Set the culture
        /// </summary>
        public string SetCulture(string language)
        {
            string culture = "";
            // Default language is "en", french is "fr" and spanish is "es".
            
            // Switch statement that choose the culture based on the language parameter.
            switch (language)
            {
                case "French":
                    culture = "fr"; // Code to be executed if
                    return culture; // language  = "French".
                case "Spanish":
                    culture = "es"; // Code to be executed if
                    return culture; // language  = "Spanish".
                default: 
                    culture = "en"; // Code to be executed for all others case 
                    return culture; // language = "English" (default).
            }
        }

        /// <summary>
        /// Update the culture cookie
        /// </summary>
        public void UpdateCultureCookie(HttpContext context, string culture)
        {
            context.Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)));
        }
    }
}
