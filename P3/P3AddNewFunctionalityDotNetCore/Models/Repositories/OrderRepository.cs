using Microsoft.EntityFrameworkCore;
using P3AddNewFunctionalityDotNetCore.Data;
using P3AddNewFunctionalityDotNetCore.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace P3AddNewFunctionalityDotNetCore.Models.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly P3Referential _context;

        public OrderRepository(P3Referential context)
        {
            _context = context;
        }

        public void Save(Order order)
        {
            if (_context != null)
            {
                if (order != null)
                {
                    if (order.OrderLine.Count != 0)
                    {
                        _context.Order.Add(order);
                        _context.SaveChanges();
                    }
                }
            }
        }

        public async Task<Order> GetOrder(int? id)
        {
            if (_context != null)
            {
                var orderEntity = await _context.Order.Include(x => x.OrderLine)
                    .ThenInclude(product => product.Product).SingleOrDefaultAsync(m => m.Id == id);
                return orderEntity;
            }

            return null;
        }

        public async Task<IList<Order>> GetOrders()
        {
            if (_context != null)
            {
                var orders = await _context.Order.Include(x => x.OrderLine)
                    .ThenInclude(product => product.Product).ToListAsync();
                return orders;
            }

            return null;
        }
    }
}