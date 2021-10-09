using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using The_Car_Hub.Data;

namespace The_Car_Hub.Models.Repositories
{
    public class MediaRepository : IMediaRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<MediaRepository> _logger;

        public MediaRepository(ApplicationDbContext context,ILogger<MediaRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IQueryable<Media> GetAllMediaByInventoryId(int id)
        {
            IQueryable<Media> medias = _context.Media.Where(x => x.InventoryId == id);
            return medias;
        }

        public async Task SaveMedia(Media media)
        {
                _context.Media.Add(media);
                await _context.SaveChangesAsync();
        }

        public async Task DeleteMedia(int id)
        {
            Media media = _context.Media.Find(id);
            _context.Media.Remove(media);
            await _context.SaveChangesAsync();
        }
    }
}