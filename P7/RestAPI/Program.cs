using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using Microsoft.Extensions.DependencyInjection;
using RestAPI.Models;
using RestAPI.Properties;

namespace RestAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile(Resources.appsettings)
                .Build();

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();

            try
            {
                Log.Information("Application Starting Up");
                var host = CreateHostBuilder(args).Build();
                using var scope = host.Services.CreateScope();
                var services = scope.ServiceProvider;
                EnsureSeedDb(services);
                host.Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex,"The application failed to start correctly.");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static void EnsureSeedDb(IServiceProvider services)
        {
            try
            {
                var userContext = services.GetRequiredService<IdentityDbContext>();
                var apiContext = services.GetRequiredService<ApplicationDbContext>();

                Log.Information("EnsureCreated() x2");
                userContext.Database.EnsureCreated();
                apiContext.Database.EnsureCreated();
            }
            catch (Exception ex)
            {
                Log.Error(string.Format("An error occurred seeding the DB. : {0}",ex.Message));
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>());
        }
    }
}