using System.Threading.Tasks;
using The_Car_Hub.Data;

namespace The_Car_Hub.Models.Repositories
{
    public interface ICarRepository
    {
        Task<Car> GetCarByInventoryIdAsync(int id);

        Task SaveCar(Car car);

        Task UpdateCar(Car car);

        Task DeleteCar(int id);
    }
}