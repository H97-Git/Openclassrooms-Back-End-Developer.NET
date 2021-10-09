using AutoMapper;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using The_Car_Hub.Data;
using The_Car_Hub.Models.Repositories;

namespace The_Car_Hub.Models.Services
{
    public class InventoryService : IInventoryService
    {
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IInventoryRepository _inventoryRepository;
        private readonly ICarRepository _carRepository;
        private readonly IInventoryStatusRepository _inventoryStatusRepository;
        private readonly IMediaRepository _mediaRepository;

        public InventoryService(IMapper mapper,IInventoryRepository inventoryRepository,
            ICarRepository carRepository,IInventoryStatusRepository inventoryStatusRepository,
            IMediaRepository mediaRepository,IWebHostEnvironment hostEnvironment)
        {
            _mapper = mapper;
            _inventoryRepository = inventoryRepository;
            _carRepository = carRepository;
            _inventoryStatusRepository = inventoryStatusRepository;
            _mediaRepository = mediaRepository;
            _hostEnvironment = hostEnvironment;
        }

        public async Task<InventoryViewModel> MapInventoryViewModelAsync(int id)
        {
            if (id != 0)
            {
                Inventory inventory = await _inventoryRepository.GetInventoryByIdAsync(id);
                if (inventory != null)
                {
                    inventory.Car = await _carRepository.GetCarByInventoryIdAsync(inventory.Id);
                    inventory.InventoryStatus = await _inventoryStatusRepository.GetStatusByInventoryIdAsync(inventory.Id);

                    InventoryViewModel inventoryViewModel = _mapper.Map<InventoryViewModel>(inventory);
                    _mapper.Map(inventory.Car,inventoryViewModel);
                    _mapper.Map(inventory.InventoryStatus,inventoryViewModel);

                    IQueryable<Media> medias = _mediaRepository.GetAllMediaByInventoryId(inventory.Id);

                    inventoryViewModel.PhotosName = (from item in medias
                                                     select item.FileName).ToList();
                    return inventoryViewModel;
                }
            }

            return null;
        }

        public async Task<List<InventoryViewModel>> GetAllInventoryAsync()
        {
            List<InventoryViewModel> listInventoryViewModel = new List<InventoryViewModel>();
            List<Inventory> inventories = await _inventoryRepository.GetAllInventoryAsync();

            foreach (Inventory inventory in inventories)
            {
                listInventoryViewModel.Add(await MapInventoryViewModelAsync(inventory.Id));
            }

            return listInventoryViewModel;
        }

        public async Task<int> CountInventoryAsync()
        {
            var inventories = await GetAllInventoryAsync();

            return inventories.Count();
        }

        public async Task<bool> VerifyVIN(string VIN)
        {
            var inventories = await GetAllInventoryAsync();
            inventories = inventories.FindAll(x => x.VIN == VIN);

            if (inventories.Count == 0)
            {
                return true;
            }

            return false;
        }

        public Inventory MapInventory(InventoryViewModel inventoryViewModel)
        {
            Inventory inventory = _mapper.Map<Inventory>(inventoryViewModel);
            inventory.Car = _mapper.Map<Car>(inventoryViewModel);
            inventory.InventoryStatus = _mapper.Map<InventoryStatus>(inventoryViewModel);

            return inventory;
        }

        public List<Media> UploadPhoto(InventoryViewModel inventoryViewModel)
        {
            List<Media> medias = new List<Media>();
            if (inventoryViewModel.Photos != null && inventoryViewModel.Photos.Count > 0)
            {
                foreach (IFormFile photo in inventoryViewModel.Photos)
                {
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + photo.FileName;
                    string uploadsFolder = Path.Combine(_hostEnvironment.WebRootPath,"upload");
                    string filePath = Path.Combine(uploadsFolder,uniqueFileName);
                    photo.CopyTo(new FileStream(filePath,FileMode.Create));
                    medias.Add(new Media() { FileName = uniqueFileName,Inventory = MapInventory(inventoryViewModel) });
                }
            }

            return medias;
        }

        public async Task SaveInventory(InventoryViewModel inventoryViewModel)
        {
            Inventory inventory = MapInventory(inventoryViewModel);

            await _inventoryStatusRepository.SaveStatus(inventory.InventoryStatus);
            await _carRepository.SaveCar(inventory.Car);
            await _inventoryRepository.SaveInventory(inventory);

            List<Media> medias = UploadPhoto(inventoryViewModel);

            foreach (var media in medias)
            {
                media.InventoryId = inventory.Id;
                media.Inventory = null;
                await _mediaRepository.SaveMedia(media);
            }
        }

        public async void DeletePhoto(List<Media> medias)
        {
            foreach (var media in medias)
            {
                await _mediaRepository.DeleteMedia(media.Id);
            }
        }

        public async Task<List<string>> GetUnusedImage()
        {
            List<InventoryViewModel> inventories = await GetAllInventoryAsync();
            string uploadsFolder = Path.Combine(_hostEnvironment.WebRootPath,"upload");
            List<string> filesPath = new List<string>();

            foreach (var item in Directory.EnumerateFiles(uploadsFolder))
            {
                filesPath.Add(item);
            }

            foreach (var item in inventories)
            {
                foreach (var medias in item.PhotosName)
                {
                    string filePath = Path.Combine(uploadsFolder,medias);
                    filesPath.Remove(filePath);
                }
            }

            return filesPath;
        }

        public async Task DeleteUnusedImage()
        {
            var filesPath = await GetUnusedImage();

            foreach (var item in filesPath)
            {
                File.Delete(item);
            }
        }

        public async Task UpdateInventory(InventoryViewModel inventoryViewModel)
        {
            Inventory inventory = MapInventory(inventoryViewModel);
            if (inventoryViewModel.Photos != null && inventoryViewModel.Photos.Count > 0)
            {
                List<Media> OldPhoto = _mediaRepository.GetAllMediaByInventoryId(inventory.Id).ToList();
                DeletePhoto(OldPhoto);
                List<Media> medias = UploadPhoto(inventoryViewModel);
                foreach (var media in medias)
                {
                    media.InventoryId = inventory.Id;
                    media.Inventory = null;
                    await _mediaRepository.SaveMedia(media);
                }
            }

            await _inventoryRepository.UpdateInventory(inventory);
            await _carRepository.UpdateCar(inventory.Car);
            await _inventoryStatusRepository.UpdateStatus(inventory.InventoryStatus);
        }

        public async Task DeleteInventory(int id)
        {
            await _inventoryRepository.DeleteInventory(id);
            await _carRepository.DeleteCar(id);
            await _inventoryStatusRepository.DeleteStatus(id);
        }

        public async Task DeleteAll()
        {
            List<InventoryViewModel> inventories = await GetAllInventoryAsync();

            foreach (var item in inventories)
            {
                await DeleteInventory(item.Id);
            }
        }

        public async Task<byte[]> Export()
        {
            List<InventoryViewModel> inventories = await GetAllInventoryAsync();

            try
            {
                using var workbook = new XLWorkbook();
                IXLWorksheet worksheet =
                workbook.Worksheets.Add("Cars");
                worksheet.Cell(1,1).Value = "Id";
                worksheet.Cell(1,2).Value = "VIN";
                worksheet.Cell(1,3).Value = "Year";
                worksheet.Cell(1,4).Value = "Make";
                worksheet.Cell(1,5).Value = "Model";
                worksheet.Cell(1,6).Value = "Trim";
                worksheet.Cell(1,7).Value = "Color";
                worksheet.Cell(1,8).Value = "Gearbox";
                worksheet.Cell(1,9).Value = "Fuel";
                worksheet.Cell(1,10).Value = "Engine Size";
                worksheet.Cell(1,11).Value = "Mileage";
                worksheet.Cell(1,12).Value = "Numbers of seats";
                worksheet.Cell(1,13).Value = "Purchase date";
                worksheet.Cell(1,14).Value = "Purchase price";
                worksheet.Cell(1,15).Value = "Repairs";
                worksheet.Cell(1,16).Value = "Repairs cost";
                worksheet.Cell(1,17).Value = "Lot date";
                worksheet.Cell(1,18).Value = "Sale date";
                worksheet.Cell(1,19).Value = "Selling price";
                worksheet.Cell(1,20).Value = "Type";
                worksheet.Cell(1,21).Value = "Status";
                for (int index = 1; index <= inventories.Count; index++)
                {
                    worksheet.Cell(index + 1,1).Value =
                    inventories[index - 1].Id;
                    worksheet.Cell(index + 1,2).Value =
                    inventories[index - 1].VIN;
                    worksheet.Cell(index + 1,3).Value =
                    inventories[index - 1].Year;
                    worksheet.Cell(index + 1,4).Value =
                    inventories[index - 1].Make;
                    worksheet.Cell(index + 1,5).Value =
                    inventories[index - 1].Model;
                    worksheet.Cell(index + 1,6).Value =
                    inventories[index - 1].Trim;
                    worksheet.Cell(index + 1,7).Value =
                    inventories[index - 1].Color;
                    worksheet.Cell(index + 1,8).Value =
                    inventories[index - 1].Gearbox;
                    worksheet.Cell(index + 1,9).Value =
                    inventories[index - 1].Fuel;
                    worksheet.Cell(index + 1,10).Value =
                    inventories[index - 1].EngineSize;
                    worksheet.Cell(index + 1,11).Value =
                    inventories[index - 1].Kilometer;
                    worksheet.Cell(index + 1,12).Value =
                    inventories[index - 1].Seats;
                    worksheet.Cell(index + 1,13).Value =
                    inventories[index - 1].PurchaseDate;
                    worksheet.Cell(index + 1,14).Value =
                    inventories[index - 1].PurchasePrice;
                    worksheet.Cell(index + 1,15).Value =
                    inventories[index - 1].Repairs;
                    worksheet.Cell(index + 1,16).Value =
                    inventories[index - 1].RepairsCost;
                    worksheet.Cell(index + 1,17).Value =
                    inventories[index - 1].LotDate;
                    worksheet.Cell(index + 1,18).Value =
                    inventories[index - 1].SaleDate;
                    worksheet.Cell(index + 1,19).Value =
                    inventories[index - 1].SellingPrice;
                    worksheet.Cell(index + 1,20).Value =
                    inventories[index - 1].Type;
                    worksheet.Cell(index + 1,21).Value =
                    inventories[index - 1].Status;
                }

                using var stream = new MemoryStream();
                workbook.SaveAs(stream);
                var content = stream.ToArray();

                return content;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}