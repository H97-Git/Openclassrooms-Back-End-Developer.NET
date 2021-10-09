using System.Linq;
using System.Threading.Tasks;
using The_Car_Hub.Data;

namespace The_Car_Hub.Models.Repositories
{
    public class CarRepository : ICarRepository
    {
        private readonly ApplicationDbContext _context;

        public CarRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Car> GetCarByInventoryIdAsync(int id)
        {
            Car car = await _context.Car.FindAsync(id);

            if (car != null)
            {
                return car;
            }
            else
            {
                return null;
            }
        }

        public async Task SaveCar(Car car)
        {
            if (car != null)
            {
                _context.Car.Add(car);
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateCar(Car car)
        {
            if (car != null)
            {
                _context.Car.Update(car);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteCar(int id)
        {
            Car car = _context.Car.FirstOrDefault(i => i.Id == id);
            if (car != null)
            {
                _context.Car.Remove(car);
                await _context.SaveChangesAsync();
            }
        }
    }
}