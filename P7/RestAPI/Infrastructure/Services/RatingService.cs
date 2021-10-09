using System.Collections.Generic;
using System.Threading.Tasks;
using Mapster;
using RestAPI.Infrastructure.Repositories;
using RestAPI.Models;
using RestAPI.Models.DTO;
using RestAPI.Properties;

namespace RestAPI.Infrastructure.Services
{
    public class RatingService : IRatingService
    {
        private readonly IRatingRepository _ratingRepository;

        public RatingService(IRatingRepository ratingRepository)
        {
            _ratingRepository = ratingRepository;
        }

        public async Task<IList<RatingDto>> GetRating()
        {
            var ratings = await _ratingRepository.GetRating();
            return ratings.Adapt<IList<RatingDto>>();
        }

        public async Task<RatingDto> GetRating(int id)
        {
            var rating = await _ratingRepository.GetRating(id);
            
            return rating == null
                ? throw new KeyNotFoundException(Resources.RatingNotFound)
                : rating.Adapt<RatingDto>();
        }

        public async Task UpdateRating(RatingDto ratingDto)
        {
            var rating = await _ratingRepository.GetRating(ratingDto.Id);
            if (rating == null)
            {
                throw new KeyNotFoundException(Resources.RatingNotFound);
            }

            ratingDto.Adapt(rating);
            await _ratingRepository.UpdateRating(rating);
        }

        public async Task SaveRating(RatingDto ratingDto)
        {
            var rating = ratingDto.Adapt<Rating>();
            await _ratingRepository.SaveRating(rating);
        }

        public async Task DeleteRating(int id)
        {
            var rating = await _ratingRepository.GetRating(id);
            if (rating == null)
            {
                throw new KeyNotFoundException(Resources.RatingNotFound); 
            }
            await _ratingRepository.DeleteRating(id);
        }
    }
}