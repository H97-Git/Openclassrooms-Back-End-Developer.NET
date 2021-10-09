using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using The_Car_Hub.Data;

namespace The_Car_Hub.Models.Repositories
{
    public class InventoryStatusRepository : IInventoryStatusRepository
    {
        private readonly ApplicationDbContext _context;

        public InventoryStatusRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<InventoryStatus> GetStatusByInventoryIdAsync(int inventoryId)
        {
            InventoryStatus status = await _context.InventoryStatus.FirstOrDefaultAsync(x => x.Id == inventoryId);
            return status;
        }

        public async Task SaveStatus(InventoryStatus status)
        {
            _context.InventoryStatus.Add(status);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateStatus(InventoryStatus status)
        {
            _context.InventoryStatus.Update(status);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteStatus(int id)
        {
            InventoryStatus status = _context.InventoryStatus.Find(id);
            _context.InventoryStatus.Remove(status);
            await _context.SaveChangesAsync();
        }
    }
}