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
    public class TradeService : ITradeService
    {
        private readonly ITradeRepository _tradeRepository;

        public TradeService(ITradeRepository tradeRepository)
        {
            _tradeRepository = tradeRepository;
        }

        public async Task<IList<TradeDto>> GetTrade()
        {
            var trades = await _tradeRepository.GetTrade();

            return trades.Adapt<IList<TradeDto>>();
        }

        public async Task<TradeDto> GetTrade(int id)
        {
            var trade = await _tradeRepository.GetTrade(id);

            return trade == null
                ? throw new KeyNotFoundException(Resources.TradeNotFound)
                : trade.Adapt<TradeDto>();
        }

        public async Task UpdateTrade((string Id, string Role) user,TradeDto tradeDto)
        {
            var trade = await _tradeRepository.GetTrade(tradeDto.Id);
            if (user.Role == "Admin" && trade != null || trade != null && user.Id == trade.OwnerId)
            {
                tradeDto.Adapt(trade);
                await _tradeRepository.UpdateTrade(trade);
            }
            else if (trade == null)
            {
                throw new KeyNotFoundException(Resources.TradeNotFound);
            }
            else if (user.Id != trade.OwnerId)
            {
                throw new AuthenticationException(Resources.TradeOwnerEdit);
            }
        }

        public async Task SaveTrade((string Id, string Role) user,TradeDto tradeDto)
        {
            var trade = tradeDto.Adapt<Trade>();
            trade.OwnerId = user.Id;
            await _tradeRepository.SaveTrade(trade);
        }

        public async Task DeleteTrade((string Id, string Role) user,int id)
        {
            var trade = await _tradeRepository.GetTrade(id);
            if (user.Role == "Admin" && trade != null || trade != null && user.Id == trade.OwnerId)
            {
                await _tradeRepository.DeleteTrade(id);
            }
            else if (trade == null)
            {
                throw new KeyNotFoundException(Resources.TradeNotFound);
            }
            else if (user.Id != trade.OwnerId)
            {
                throw new AuthenticationException(Resources.TradeOwnerDelete);
            }
        }
    }
}