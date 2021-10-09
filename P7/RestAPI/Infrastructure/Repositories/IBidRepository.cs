using System.Collections.Generic;
using System.Threading.Tasks;
using RestAPI.Models;

namespace RestAPI.Infrastructure.Repositories
{
    public interface IBidRepository
    {
        Task<IList<Bid>> GetBid();

        Task<Bid> GetBid(int id);

        Task UpdateBid(Bid bid);

        Task SaveBid(Bid bid);

        Task DeleteBid(int id);
    }
}