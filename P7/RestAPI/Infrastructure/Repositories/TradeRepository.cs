using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RestAPI.Models;

namespace RestAPI.Infrastructure.Repositories
{
    public class TradeRepository : ITradeRepository
    {
        private static ApplicationDbContext _applicationDbContext;

        public TradeRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<IList<Trade>> GetTrade()
        {
            var trade = await _applicationDbContext.Trades.ToListAsync();
            return trade;
        }

        public async Task<Trade> GetTrade(int id)
        {
            var trade = await _applicationDbContext.Trades.FindAsync(id);
            return trade;
        }

        public async Task UpdateTrade([Required] Trade trade)
        {
            _applicationDbContext.Trades.Update(trade);
            await _applicationDbContext.SaveChangesAsync();
        }

        public async Task SaveTrade([Required] Trade trade)
        {
            await _applicationDbContext.Trades.AddAsync(trade);
            await _applicationDbContext.SaveChangesAsync();
        }

        public async Task DeleteTrade(int id)
        {
            var trade = await _applicationDbContext.Trades.FindAsync(id);
            _applicationDbContext.Trades.Remove(trade);
            await _applicationDbContext.SaveChangesAsync();
        }
    }
}