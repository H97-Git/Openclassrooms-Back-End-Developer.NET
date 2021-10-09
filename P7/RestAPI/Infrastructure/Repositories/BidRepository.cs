using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RestAPI.Models;

namespace RestAPI.Infrastructure.Repositories
{
    public class BidRepository : IBidRepository
    {
        private static ApplicationDbContext _applicationDbContext;

        public BidRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<IList<Bid>> GetBid()
        {
            var bids = await _applicationDbContext.Bids.ToListAsync();

            return bids;
        }

        public async Task<Bid> GetBid(int id)
        {
            var bid = await _applicationDbContext.Bids.FindAsync(id);

            return bid;
        }

        public async Task UpdateBid([Required] Bid bid)
        {
            _applicationDbContext.Bids.UpdateRange(bid);
            await _applicationDbContext.SaveChangesAsync();
        }

        public async Task SaveBid([Required] Bid bid)
        {
            await _applicationDbContext.Bids.AddAsync(bid);
            await _applicationDbContext.SaveChangesAsync();
        }

        public async Task DeleteBid(int id)
        {
            var bid = await _applicationDbContext.Bids.FindAsync(id);
            _applicationDbContext.Bids.Remove(bid);
            await _applicationDbContext.SaveChangesAsync();
        }
    }
}