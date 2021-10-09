using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using The_Car_Hub.Data;

namespace The_Car_Hub.Models.Repositories
{
    public class InventoryRepository : IInventoryRepository
    {
        private readonly ApplicationDbContext _context;

        public InventoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Inventory>> GetAllInventoryAsync()
        {
            List<Inventory> inventoryEntities = await _context.Inventory.ToListAsync();
            return inventoryEntities.ToList();
        }

        public async Task<Inventory> GetInventoryByIdAsync(int id)
        {
            Inventory inventory = await _context.Inventory.FindAsync(id);

            if (inventory != null)
            {
                return inventory;
            }
            else
            {
                return null;
            }
        }

        public async Task SaveInventory(Inventory inventory)
        {
            if (inventory != null)
            {
                _context.Inventory.Add(inventory);
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateInventory(Inventory inventory)
        {
            if (inventory != null)
            {
                _context.Inventory.Update(inventory);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteInventory(int id)
        {
            Inventory inventory = _context.Inventory.FirstOrDefault(i => i.Id == id);
            if (inventory != null)
            {
                _context.Inventory.Remove(inventory);
                await _context.SaveChangesAsync();
            }
        }
    }
}