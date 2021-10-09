using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RestAPI.Models;

namespace RestAPI.Infrastructure.Repositories
{
    public class RatingRepository : IRatingRepository
    {
        private static ApplicationDbContext _applicationDbContext;

        public RatingRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<IList<Rating>> GetRating()
        {
            var rating = await _applicationDbContext.Ratings.ToListAsync();
            return rating;
        }

        public async Task<Rating> GetRating(int id)
        {
            var rating = await _applicationDbContext.Ratings.FindAsync(id);
            return rating;
        }

        public async Task UpdateRating([Required] Rating rating)
        {
            _applicationDbContext.Ratings.Update(rating);
            await _applicationDbContext.SaveChangesAsync();
        }

        public async Task SaveRating([Required] Rating rating)
        {
            await _applicationDbContext.Ratings.AddAsync(rating);
            await _applicationDbContext.SaveChangesAsync();
        }

        public async Task DeleteRating(int id)
        {
            var rating = await _applicationDbContext.Ratings.FindAsync(id);
            _applicationDbContext.Ratings.Remove(rating);
            await _applicationDbContext.SaveChangesAsync();
        }
    }
}