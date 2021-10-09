using CalifornianHealth.Demographics.Config;
using CalifornianHealth.Demographics.Data;
using CalifornianHealth.Demographics.Infrastructure.Repositories;
using CalifornianHealth.Demographics.Infrastructure.Repositories.Interface;
using CalifornianHealth.Demographics.Infrastructure.Services;
using CalifornianHealth.Demographics.Infrastructure.Services.Interface;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace CalifornianHealth.Demographics
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

            services.AddControllers();

            services.AddTransient<IPatientRepository, PatientRepository>();
            services.AddTransient<IConsultantRepository, ConsultantRepository>();

            services.AddTransient<IPatientService, PatientService>();
            services.AddTransient<IConsultantService, ConsultantService>();

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

            var connectionString =
                Configuration.GetConnectionString(_env.IsDevelopment() ? "DemogDB" : "DockerDemogDB");
            services.AddDbContext<DemographicsContext>(options => options.UseSqlServer(connectionString));

            services.AddCustomSwagger();
            services.AddMediatR(typeof(Startup).Assembly);
        }

        public void Configure(IApplicationBuilder app)
        {
            Log.Information("Startup : Configure()");
            app.UseSerilogRequestLogging();

            app.UseCors("default");
            app.UseCustomExceptionHandler();
            app.UseCustomSwagger();
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => { endpoints.MapDefaultControllerRoute(); });
        }
    }
}