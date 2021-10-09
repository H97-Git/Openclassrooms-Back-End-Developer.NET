using System.Collections.Generic;
using System.Threading.Tasks;
using RestAPI.Models.DTO;

namespace RestAPI.Infrastructure.Services
{
    public interface ITradeService
    {
        Task<IList<TradeDto>> GetTrade();

        Task<TradeDto> GetTrade(int id);

        Task UpdateTrade((string Id, string Role) user,TradeDto tradeDto);

        Task SaveTrade((string Id, string Role) user,TradeDto tradeDto);

        Task DeleteTrade((string Id, string Role) user,int id);
    }
}