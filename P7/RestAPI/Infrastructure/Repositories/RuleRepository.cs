using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RestAPI.Models;

namespace RestAPI.Infrastructure.Repositories
{
    public class RuleRepository : IRuleRepository
    {
        private static ApplicationDbContext _applicationDbContext;

        public RuleRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<IList<Rule>> GetRule()
        {
            var rule = await _applicationDbContext.Rules.ToListAsync();
            return rule;
        }

        public async Task<Rule> GetRule(int id)
        {
            var rule = await _applicationDbContext.Rules.FindAsync(id);
            return rule;
        }

        public async Task UpdateRule([Required] Rule rule)
        {
            _applicationDbContext.Rules.Update(rule);
            await _applicationDbContext.SaveChangesAsync();
        }

        public async Task SaveRule([Required] Rule rule)
        {
            await _applicationDbContext.Rules.AddAsync(rule);
            await _applicationDbContext.SaveChangesAsync();
        }

        public async Task DeleteRule(int id)
        {
            var rule = await _applicationDbContext.Rules.FindAsync(id);
            _applicationDbContext.Rules.Remove(rule);
            await _applicationDbContext.SaveChangesAsync();
        }
    }
}