using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CalifornianHealth.Demographics.Data;
using CalifornianHealth.Demographics.Data.DTO;
using CalifornianHealth.Demographics.Infrastructure.Repositories.Interface;
using CalifornianHealth.Demographics.Infrastructure.Services.Interface;
using Mapster;

namespace CalifornianHealth.Demographics.Infrastructure.Services
{
    public class ConsultantService : IConsultantService
    {
        private static IConsultantRepository _consultantRepository;

        public ConsultantService(IConsultantRepository consultantRepository)
        {
            _consultantRepository = consultantRepository;
        }

        public async Task<List<ConsultantDto>> GetConsultant()
        {
            var consultants = await _consultantRepository.GetConsultant();

            return consultants.Adapt<List<ConsultantDto>>();
        }

        public async Task<ConsultantDto> GetConsultant(int id)
        {
            var consultant = await _consultantRepository.GetConsultant(id);

            return consultant is null
                ? throw new KeyNotFoundException($"Consultant : {id}")
                : consultant.Adapt<ConsultantDto>();
        }

        public async Task UpdateConsultant(ConsultantDto consultantDto)
        {
            if (consultantDto is null) throw new ArgumentNullException(nameof(consultantDto));

            var consultant = await _consultantRepository.GetConsultant(consultantDto.Id);
            if (consultant is null) throw new KeyNotFoundException($"Consultant : {consultantDto.Id}");

            consultantDto.Adapt(consultant);
            await _consultantRepository.UpdateConsultant(consultant);
        }

        public async Task SaveConsultant(ConsultantDto consultantDto)
        {
            if (consultantDto is null) throw new ArgumentNullException(nameof(consultantDto));

            var consultant = consultantDto.Adapt<Consultant>();
            await _consultantRepository.SaveConsultant(consultant);
        }

        public async Task DeleteConsultant(int id)
        {
            var consultant = await _consultantRepository.GetConsultant(id);
            if (consultant is null) throw new KeyNotFoundException($"Consultant : {id}");

            await _consultantRepository.DeleteConsultant(consultant);
        }
    }
}