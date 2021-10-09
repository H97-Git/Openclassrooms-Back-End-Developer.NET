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
    public class PatientService : IPatientService
    {
        private static IPatientRepository _patientRepository;

        public PatientService(IPatientRepository patientRepository)
        {
            _patientRepository = patientRepository;
        }

        public async Task<List<PatientDto>> GetPatient()
        {
            var patients = await _patientRepository.GetPatient();

            return patients.Adapt<List<PatientDto>>();
        }

        public async Task<PatientDto> GetPatient(int id)
        {
            var patient = await _patientRepository.GetPatient(id);

            return patient is null
                ? throw new KeyNotFoundException($"Patient : {id}")
                : patient.Adapt<PatientDto>();
        }

        public async Task UpdatePatient(PatientDto patientDto)
        {
            if (patientDto is null) throw new ArgumentNullException(nameof(patientDto));

            var patient = await _patientRepository.GetPatient(patientDto.Id);
            if (patient is null) throw new KeyNotFoundException($"Patient {patientDto.Id}");

            patientDto.Adapt(patient);
            await _patientRepository.UpdatePatient(patient);
        }

        public async Task SavePatient(PatientDto patientDto)
        {
            if (patientDto is null) throw new ArgumentNullException(nameof(patientDto));

            var patient = patientDto.Adapt<Patient>();
            await _patientRepository.SavePatient(patient);
        }

        public async Task DeletePatient(int id)
        {
            var patient = await _patientRepository.GetPatient(id);
            if (patient is null) throw new KeyNotFoundException($"Patient : {id}");

            await _patientRepository.DeletePatient(patient);
        }
    }
}