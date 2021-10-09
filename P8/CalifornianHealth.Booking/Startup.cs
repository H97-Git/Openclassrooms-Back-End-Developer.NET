using System.Threading.Tasks;
using CalifornianHealth.Booking.BackgroundServices;
using CalifornianHealth.Booking.Config;
using CalifornianHealth.Booking.Data;
using CalifornianHealth.Booking.Infrastructure.Repositories;
using CalifornianHealth.Booking.Infrastructure.Repositories.Interface;
using CalifornianHealth.Booking.Infrastructure.Services;
using CalifornianHealth.Booking.Infrastructure.Services.Interface;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace CalifornianHealth.Booking
{
    public class Startup
    {
        private readonly IWebHostEnvironment _env;

        public Startup(IWebHostEnvironment env, IConfiguration configuration)
        {
            _env = env;
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            Log.Information("Startup : ConfigureServices()");

            var connectionString =
                Configuration.GetConnectionString(_env.IsDevelopment() ? "BookingDB" : "DockerBookingDB");
            Log.Debug($"connectionString : {connectionString}");

            services.AddDbContext<BookingContext>(options => options.UseSqlServer(connectionString));

            services.AddTransient<IBookingRepository, BookingRepository>();
            services.AddTransient<IAvailabilityRepository, AvailabilityRepository>();

            services.AddTransient<IBookingService, BookingService>();
            services.AddTransient<IAvailabilityService, AvailabilityService>();

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

            services.AddControllers();
            services.AddCustomSwagger();
            services.AddMediatR(typeof(Startup).Assembly);
            Log.Debug($"_env.IsDevelopment() : {_env.IsDevelopment()}");
            var rabbitSection = Configuration.GetSection("RabbitSection");
            var rabbitConnectionString =
                $"host={rabbitSection.GetValue<string>(_env.IsDevelopment() ? "HostName" : "HostNameDocker")};" +
                $"username={rabbitSection.GetValue<string>("UserName")};" +
                $"password={rabbitSection.GetValue<string>("Password")}";
            Log.Debug($"rabbitConnectionString : {rabbitConnectionString}");

            services.RegisterEasyNetQ(rabbitConnectionString);
            services.AddHostedService<BookingEventHandler>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            Log.Information("Startup : Configure()");
            app.UseSerilogRequestLogging();

            app.UseCors("default");
            app.UseCustomExceptionHandler();
            app.UseCustomSwagger();
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("",
                    context => Task.Factory.StartNew(() =>
                        context.Response.Redirect("/swagger/index.html", true, true)));
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}