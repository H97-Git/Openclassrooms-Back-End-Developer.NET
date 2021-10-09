using CalifornianHealth.WebBlazor.Infrastructure.Services;
using CalifornianHealth.WebBlazor.Infrastructure.Services.Interface;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace CalifornianHealth.WebBlazor
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
        }

        public void ConfigureServices(IServiceCollection services)
        {
            Log.Information("Startup : ConfigureServices() ...");
            services.AddControllersWithViews();
            services.AddRazorPages();
            services.AddServerSideBlazor();

            services.AddHttpClient();
            services.AddScoped<IConsultantService, ConsultantService>();
            services.AddScoped<IAvailabilityService, AvailabilityService>();
            services.AddScoped<IBookingService, BookingService>();

            services.AddCors(options =>
            {
                options.AddPolicy("default", policy =>
                {
                    policy.WithOrigins("http://localhost:5000", "http://localhost:6000", "http://localhost:7000",
                            "http://localhost:8080")
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            Log.Information("Startup : Configure() ...");
            app.UseSerilogRequestLogging();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseCors("default");
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}