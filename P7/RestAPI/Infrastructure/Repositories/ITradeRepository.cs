using System.Collections.Generic;
using System.Threading.Tasks;
using RestAPI.Models;

namespace RestAPI.Infrastructure.Repositories
{
    public interface ITradeRepository
    {
        Task<IList<Trade>> GetTrade();

        Task<Trade> GetTrade(int id);

        Task UpdateTrade(Trade trade);

        Task SaveTrade(Trade trade);

        Task DeleteTrade(int id);
    }
}