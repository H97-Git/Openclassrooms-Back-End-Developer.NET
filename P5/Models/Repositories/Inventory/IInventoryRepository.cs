using System.Collections.Generic;
using System.Threading.Tasks;
using The_Car_Hub.Data;

namespace The_Car_Hub.Models.Repositories
{
    public interface IInventoryRepository
    {
        Task<List<Inventory>> GetAllInventoryAsync();

        Task<Inventory> GetInventoryByIdAsync(int id);

        Task SaveInventory(Inventory inventory);

        Task UpdateInventory(Inventory inventory);

        Task DeleteInventory(int id);
    }
}