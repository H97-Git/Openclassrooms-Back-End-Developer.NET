using System.Collections.Generic;
using System.Threading.Tasks;
using RestAPI.Models.DTO;

namespace RestAPI.Infrastructure.Services
{
    public interface IRuleService
    {
        Task<IList<RuleDto>> GetRule();

        Task<RuleDto> GetRule(int id);

        Task UpdateRule(RuleDto ruleDto);

        Task SaveRule(RuleDto ruleDto);

        Task DeleteRule(int id);
    }
}