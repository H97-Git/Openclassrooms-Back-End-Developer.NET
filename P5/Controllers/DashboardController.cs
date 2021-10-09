using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using The_Car_Hub.Helper;
using The_Car_Hub.Models;
using The_Car_Hub.Models.Services;

namespace The_Car_Hub.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly IInventoryService _inventoryService;
        private readonly IWebHostEnvironment _hostEnvironment;

        public DashboardController(IInventoryService inventoryService,IWebHostEnvironment hostEnvironment)
        {
            _inventoryService = inventoryService;
            _hostEnvironment = hostEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            List<InventoryViewModel> model = await _inventoryService.GetAllInventoryAsync();
            ViewBag.UnusedImages = CountUnusedImage();

            return View(model);
        }

        public async Task<IActionResult> AddOrEdit(int id = 0)
        {
            if (id == 0)
                return View();
            else
            {
                var inventoryViewModel = await _inventoryService.MapInventoryViewModelAsync(id);

                if (inventoryViewModel == null)
                {
                    return NotFound();
                }

                return View(inventoryViewModel);
            }
        }

        [AcceptVerbs("Get","Post")]
        public async Task<IActionResult> VerifyVIN(string VIN)
        {
            if (!string.IsNullOrEmpty(VIN))
            {
                var result = await _inventoryService.VerifyVIN(VIN);
                return Json(result ? "true" : string.Format("VIN : {0} already exists.",VIN));
            }

            return Json("false");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit(int id,InventoryViewModel inventoryViewModel)
        {
            if (ModelState.IsValid)
            {
                //Insert
                if (id == 0)
                {
                    await _inventoryService.SaveInventory(inventoryViewModel);
                    ViewBag.SuccessMsg = "Successfully added";
                }
                //Update
                else
                {
                    try
                    {
                        await _inventoryService.UpdateInventory(inventoryViewModel);
                        ViewBag.SuccessMsg = "Successfully edited";
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        throw;
                    }
                }
                return Json(new { isValid = true,html = RazorHelper.RenderRazorViewToString(this,"_ViewAll",await _inventoryService.GetAllInventoryAsync()),Count = CountUnusedImage() });
            }

            return Json(new { isValid = false,html = RazorHelper.RenderRazorViewToString(this,"AddOrEdit",inventoryViewModel),Count = CountUnusedImage() });
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            await _inventoryService.DeleteInventory((int)id);
            ViewBag.SuccessMsg = $"Id : {id} Successfully deleted";

            return Json(new { isValid = true,html = RazorHelper.RenderRazorViewToString(this,"_ViewAll",await _inventoryService.GetAllInventoryAsync()),Count = CountUnusedImage() });
        }

        public async Task<IActionResult> DeleteAll()
        {
            await _inventoryService.DeleteAll();
            ViewBag.SuccessMsg = "All cars successfully deleted";

            return Json(new { isValid = true,html = RazorHelper.RenderRazorViewToString(this,"_ViewAll",await _inventoryService.GetAllInventoryAsync()),Count = CountUnusedImage() });
        }

        public async Task<IActionResult> EmptyImageFolder()
        {
            await _inventoryService.DeleteUnusedImage();
            ViewBag.SuccessMsg = "All images successfully deleted";

            return Json(new { isValid = true,html = RazorHelper.RenderRazorViewToString(this,"_ViewAll",await _inventoryService.GetAllInventoryAsync()),Count = CountUnusedImage() });
        }

        public async Task<int> CountUnusedImage()
        {
            var filesPath = await _inventoryService.GetUnusedImage();

            return filesPath.Count();
        }

        public async Task<IActionResult> Export()
        {
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            string fileName = "data.xlsx";

            var content = await _inventoryService.Export();
            ViewBag.SuccessMsg = "Successfully exported";

            return File(content,contentType,fileName);
        }
    }
}