using System.Linq;
using System.Threading.Tasks;
using The_Car_Hub.Data;

namespace The_Car_Hub.Models.Repositories
{
    public interface IMediaRepository
    {
        IQueryable<Media> GetAllMediaByInventoryId(int id);

        Task SaveMedia(Media media);

        Task DeleteMedia(int id);
    }
}