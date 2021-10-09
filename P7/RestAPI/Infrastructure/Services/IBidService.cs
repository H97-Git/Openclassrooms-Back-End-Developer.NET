using System.Collections.Generic;
using System.Threading.Tasks;
using RestAPI.Models.DTO;

namespace RestAPI.Infrastructure.Services
{
    public interface IBidService
    {
        Task<IList<BidDto>> GetBid();

        Task<BidDto> GetBid(int id);

        Task UpdateBid((string Id, string Role) user,BidDto bidDto);

        Task SaveBid((string Id, string Role) user,BidDto bidDto);

        Task DeleteBid((string Id, string Role) user,int id);
    }
}