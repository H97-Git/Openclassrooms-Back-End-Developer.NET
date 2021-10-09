using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace The_Car_Hub.Data
{
    public class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ApplicationDbContext(serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
            {
                if (context.Inventory.Any())
                {
                    return;
                }

                var inventoryStatus = new InventoryStatus
                {
                    Status = InventoryStatus.StatusEnum.Maintenance
                };
                context.InventoryStatus.Add(inventoryStatus);

                var inventoryStatus_Two = new InventoryStatus
                {
                    Status = InventoryStatus.StatusEnum.Unavaible
                };
                context.InventoryStatus.Add(inventoryStatus_Two);

                var car = new Car
                {
                    Make = "Audi",
                    Model = "A3",
                    Trim = "Sport",
                    Year = 2020,
                    Color = "White",
                    EngineSize = 10000,
                    Fuel = "Diesel",
                    Gearbox = "Manual",
                    Seats = 4
                };
                context.Car.Add(car);

                var car_Two = new Car
                {
                    Make = "Audi",
                    Model = "A4",
                    Trim = "Sport",
                    Year = 2019,
                    Color = "Black",
                    EngineSize = 10000,
                    Fuel = "Diesel",
                    Gearbox = "Manual",
                    Seats = 5
                };
                context.Car.Add(car_Two);

                var inventory = new Inventory
                {
                    Car = car,
                    VIN = "2CNDL23F856093901",
                    Kilometer = 98543,
                    PurchasePrice = 33000,
                    PurchaseDate = DateTime.Today,
                    Repairs = "None",
                    RepairsCost = 0,
                    LotDate = DateTime.Today,
                    SellingPrice = 33500,
                    InventoryStatus = inventoryStatus,
                    Type = "New vehicle",
                    Description = "Test"
                };

                var inventory_Two = new Inventory
                {
                    Car = car_Two,
                    VIN = "1FUJAPCK25DU88948",
                    Kilometer = 89374,
                    PurchasePrice = 43000,
                    PurchaseDate = DateTime.Today,
                    Repairs = "None",
                    RepairsCost = 0,
                    LotDate = DateTime.Today,
                    SellingPrice = 43500,
                    InventoryStatus = inventoryStatus_Two,
                    Type = "Used vehicle",
                    Description = "Test"
                };

                context.Inventory.Add(inventory);
                context.Inventory.Add(inventory_Two);

                context.SaveChanges();
            }
        }
    }
}