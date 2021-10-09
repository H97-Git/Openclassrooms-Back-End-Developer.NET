using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Serilog;

namespace CalifornianHealth.Demographics.Config
{
    public static class ExceptionHandler
    {
        public static IApplicationBuilder UseCustomExceptionHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(builder =>
            {
                builder.Run(async context =>
                {
                    context.Response.ContentType = "application/json";
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    var ex = context.Features.Get<IExceptionHandlerPathFeature>().Error;
                    var exType = ex.GetType().Name;
                    switch (exType)
                    {
                        case "ArgumentNullException":
                            await context.Response
                                .WriteAsJsonAsync(new {Error = "Argument was null.", Arg = ex.Message});
                            Log.Error(ex, "Resources not found.");
                            return;
                        case "KeyNotFoundException":
                            await context.Response
                                .WriteAsJsonAsync(new {Error = "Resources not found in the system.", ex.Message});
                            Log.Error(ex, "Resources not found.");
                            return;
                        default:
                            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                            await context.Response
                                .WriteAsJsonAsync(new {Error = "An unknown error has occurred."});
                            return;
                    }
                });
            });
            return app;
        }
    }
}