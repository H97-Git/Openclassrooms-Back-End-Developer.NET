using System.Collections.Generic;
using System.Threading.Tasks;
using Mapster;
using RestAPI.Infrastructure.Repositories;
using RestAPI.Models;
using RestAPI.Models.DTO;
using RestAPI.Properties;

namespace RestAPI.Infrastructure.Services
{
    public class RuleService : IRuleService
    {
        private readonly IRuleRepository _ruleRepository;

        public RuleService(IRuleRepository ruleRepository)
        {
            _ruleRepository = ruleRepository;
        }

        public async Task<IList<RuleDto>> GetRule()
        {
            var rules = await _ruleRepository.GetRule();

            return rules.Adapt<IList<RuleDto>>();
        }

        public async Task<RuleDto> GetRule(int id)
        {
            var rule = await _ruleRepository.GetRule(id);

            return rule == null
                ? throw new KeyNotFoundException(Resources.RuleNotFound)
                : rule.Adapt<RuleDto>();
        }

        public async Task UpdateRule(RuleDto ruleDto)
        {
            var rule = await _ruleRepository.GetRule(ruleDto.Id);
            if (rule == null)
            {
                throw new KeyNotFoundException(Resources.RuleNotFound);
            }

            rule.Adapt(rule);
            await _ruleRepository.UpdateRule(rule);
        }

        public async Task SaveRule(RuleDto ruleDto)
        {
            var rule = ruleDto.Adapt<Rule>();
            await _ruleRepository.SaveRule(rule);
        }

        public async Task DeleteRule(int id)
        {
            var rule = await _ruleRepository.GetRule(id);
            if (rule == null)
            {
                throw new KeyNotFoundException(Resources.RuleNotFound);
            }

            await _ruleRepository.DeleteRule(id);
        }
    }
}