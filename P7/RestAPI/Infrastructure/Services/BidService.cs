using RestAPI.Infrastructure.Repositories;
using RestAPI.Models.DTO;
using System.Collections.Generic;
using System.Security.Authentication;
using System.Threading.Tasks;
using Mapster;
using RestAPI.Models;
using RestAPI.Properties;

namespace RestAPI.Infrastructure.Services
{
    public class BidService : IBidService
    {
        private readonly IBidRepository _bidRepository;

        public BidService(IBidRepository bidRepository)
        {
            _bidRepository = bidRepository;
        }

        public async Task<IList<BidDto>> GetBid()
        {
            var bids = await _bidRepository.GetBid();

            return bids.Adapt<IList<BidDto>>();
        }

        public async Task<BidDto> GetBid(int id)
        {
            var bid = await _bidRepository.GetBid(id);

            return bid == null
                ? throw new KeyNotFoundException(Resources.BidNotFound)
                : bid.Adapt<BidDto>();
        }

        public async Task UpdateBid((string Id, string Role) user,BidDto bidDto)
        {
            var bid = await _bidRepository.GetBid(bidDto.Id);
            if (user.Role == "Admin" && bid != null || bid != null && user.Id == bid.OwnerId)
            {
                bidDto.Adapt(bid);
                await _bidRepository.UpdateBid(bid);
            }
            else if (bid == null)
            {
                throw new KeyNotFoundException(Resources.BidNotFound);
            }
            else if (user.Id != bid.OwnerId)
            {
                throw new AuthenticationException(Resources.BidOwnerEdit);
            }
        }

        public async Task SaveBid((string Id, string Role) user,BidDto bidDto)
        {
            var bid = bidDto.Adapt<Bid>();
            bid.OwnerId = user.Id;
            await _bidRepository.SaveBid(bid);
        }

        public async Task DeleteBid((string Id, string Role) user,int id)
        {
            var bid = await _bidRepository.GetBid(id);
            if (user.Role == "Admin" && bid != null || bid != null && user.Id == bid.OwnerId)
            {
                await _bidRepository.DeleteBid(id);
            }
            else if (bid == null)
            {
                throw new KeyNotFoundException(Resources.BidNotFound);
            }
            else if (user.Id != bid.OwnerId)
            {
                throw new AuthenticationException(Resources.BidOwnerDelete);
            }
        }
    }
}