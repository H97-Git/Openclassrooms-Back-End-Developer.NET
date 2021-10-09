using System.Collections.Generic;
using System.Threading.Tasks;
using RestAPI.Models.DTO;

namespace RestAPI.Infrastructure.Services
{
    public interface IRatingService
    {
        Task<IList<RatingDto>> GetRating();

        Task<RatingDto> GetRating(int id);

        Task UpdateRating(RatingDto ratingDto);

        Task SaveRating(RatingDto ratingDto);

        Task DeleteRating(int id);
    }
}