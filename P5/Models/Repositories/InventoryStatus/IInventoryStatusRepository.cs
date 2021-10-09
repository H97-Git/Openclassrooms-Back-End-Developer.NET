using System.Threading.Tasks;
using The_Car_Hub.Data;

namespace The_Car_Hub.Models.Repositories
{
    public interface IInventoryStatusRepository
    {
        Task<InventoryStatus> GetStatusByInventoryIdAsync(int id);

        Task SaveStatus(InventoryStatus status);

        Task UpdateStatus(InventoryStatus status);

        Task DeleteStatus(int id);
    }
}