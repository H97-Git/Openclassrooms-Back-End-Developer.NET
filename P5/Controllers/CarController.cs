using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using The_Car_Hub.Models;
using The_Car_Hub.Models.Services;

namespace The_Car_Hub.Controllers
{
    public class CarController : Controller
    {
        private readonly IInventoryService _inventoryService;
        private readonly IConfiguration _configuration;
        public InventoryViewModel InventoryViewModel { get; private set; }

        public CarController(IInventoryService inventoryService,IConfiguration configuration)
        {
            _inventoryService = inventoryService;
            _configuration = configuration;
        }

        public async Task<IActionResult> Index()
        {
            List<InventoryViewModel> model = await _inventoryService.GetAllInventoryAsync();
            return View(model);
        }

        public async Task<IActionResult> Details(int id)
        {
            var InventoryViewModel = await _inventoryService.MapInventoryViewModelAsync(id);

            ViewBag.Name = _configuration.GetSection("Contact").GetSection("Name").Value;
            ViewBag.Phone = _configuration.GetSection("Contact").GetSection("Phone").Value;
            ViewBag.MobilePhone = _configuration.GetSection("Contact").GetSection("Mobile phone").Value;
            ViewBag.Email = _configuration.GetSection("Contact").GetSection("Email").Value;

            if (InventoryViewModel == null)
            {
                Response.StatusCode = 404;
                throw new Exception("No Car found");
            }

            return View(InventoryViewModel);
        }

        public async Task<IActionResult> Listing()
        {
            List<InventoryViewModel> model = await _inventoryService.GetAllInventoryAsync();

            return View(model);
        }

        public IActionResult Contact()
        {
            return View();
        }
    }
}