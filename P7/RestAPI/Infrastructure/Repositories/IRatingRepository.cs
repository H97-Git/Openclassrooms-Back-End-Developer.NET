using System.Collections.Generic;
using System.Threading.Tasks;
using RestAPI.Models;

namespace RestAPI.Infrastructure.Repositories
{
    public interface IRatingRepository
    {
        Task<IList<Rating>> GetRating();

        Task<Rating> GetRating(int id);

        Task UpdateRating(Rating rating);

        Task SaveRating(Rating rating);

        Task DeleteRating(int id);
    }
}