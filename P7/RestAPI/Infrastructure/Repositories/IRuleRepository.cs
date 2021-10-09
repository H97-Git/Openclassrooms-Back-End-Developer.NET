using System.Collections.Generic;
using System.Threading.Tasks;
using RestAPI.Models;

namespace RestAPI.Infrastructure.Repositories
{
    public interface IRuleRepository
    {
        Task<IList<Rule>> GetRule();

        Task<Rule> GetRule(int id);

        Task UpdateRule(Rule rule);

        Task SaveRule(Rule rule);

        Task DeleteRule(int id);
    }
}