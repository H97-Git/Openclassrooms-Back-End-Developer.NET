using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
using The_Car_Hub.Data;
using The_Car_Hub.Models;
using The_Car_Hub.Models.Repositories;
using The_Car_Hub.Models.Services;

namespace The_Car_Hub
{
    public class Startup
    {
        public Startup(IConfiguration configuration,IWebHostEnvironment env)
        {
            Configuration = configuration;
            CurrentWebHostEnvironment = env;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.

        private IWebHostEnvironment CurrentWebHostEnvironment { get; set; }

        public void ConfigureServices(IServiceCollection services)
        {
            var connectionStringName = CurrentWebHostEnvironment.IsDevelopment() ? "DevConnection" : "ProdConnection";

            services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlServer(
                        Configuration.GetConnectionString(connectionStringName)));

            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddControllersWithViews();
            services.AddRazorPages();

            services.AddAutoMapper(typeof(Startup));

            services.AddScoped<IInventoryRepository,InventoryRepository>();
            services.AddScoped<ICarRepository,CarRepository>();
            services.AddScoped<IInventoryStatusRepository,InventoryStatusRepository>();
            services.AddScoped<IMediaRepository,MediaRepository>();
            services.AddScoped<IInventoryService,InventoryService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app,IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Error/500");
                app.UseStatusCodePagesWithReExecute("/Error/404");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Car}/{action=Index}/{id?}");
                endpoints.MapRazorPages();

                endpoints.MapGet("/Identity/Account/Register",context => Task.Factory.StartNew(() => context.Response.Redirect("/Identity/Account/Login",true,true)));
                endpoints.MapPost("/Identity/Account/Register",context => Task.Factory.StartNew(() => context.Response.Redirect("/Identity/Account/Login",true,true)));

                endpoints.MapGet("/Admin",context => Task.Factory.StartNew(() => context.Response.Redirect("/Identity/Account/Login",true,true)));
                endpoints.MapPost("/Admin",context => Task.Factory.StartNew(() => context.Response.Redirect("/Identity/Account/Login",true,true)));

                endpoints.MapGet("/Home",context => Task.Factory.StartNew(() => context.Response.Redirect("/",true,true)));
                endpoints.MapPost("/Home",context => Task.Factory.StartNew(() => context.Response.Redirect("/",true,true)));
            });

            IdentitySeedData.EnsurePopulated(app);
        }
    }
}