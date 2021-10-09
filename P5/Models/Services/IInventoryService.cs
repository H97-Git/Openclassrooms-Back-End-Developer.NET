using System.Collections.Generic;
using System.Threading.Tasks;

namespace The_Car_Hub.Models.Services
{
    public interface IInventoryService
    {
        Task<List<InventoryViewModel>> GetAllInventoryAsync();

        Task<int> CountInventoryAsync();

        Task<bool> VerifyVIN(string VIN);

        Task<InventoryViewModel> MapInventoryViewModelAsync(int id);

        Task SaveInventory(InventoryViewModel inventoryViewModel);

        Task UpdateInventory(InventoryViewModel inventoryViewModel);

        Task<List<string>> GetUnusedImage();

        Task DeleteUnusedImage();

        Task DeleteInventory(int id);

        Task DeleteAll();

        Task<byte[]> Export();
    }
}