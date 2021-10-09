using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CalifornianHealth.Booking.Data;
using CalifornianHealth.Booking.Data.DTO;
using CalifornianHealth.Booking.Infrastructure.Repositories.Interface;
using CalifornianHealth.Booking.Infrastructure.Services.Interface;
using Mapster;

namespace CalifornianHealth.Booking.Infrastructure.Services
{
    public class AvailabilityService : IAvailabilityService
    {
        private static IAvailabilityRepository _availabilityRepository;

        public AvailabilityService(IAvailabilityRepository availabilityRepository)
        {
            _availabilityRepository = availabilityRepository;
        }

        public async Task<List<AvailabilityDto>> GetAvailabilities()
        {
            var availabilities = await _availabilityRepository.GetAvailabilities();

            return availabilities.Adapt<List<AvailabilityDto>>();
        }

        public async Task<AvailabilityDto> GetAvailability(int id)
        {
            var availability = await _availabilityRepository.GetAvailability(id);

            return availability is null
                ? throw new KeyNotFoundException($"Availability : {id}")
                : availability.Adapt<AvailabilityDto>();
        }

        public async Task<List<AvailabilityDto>> FilterByConsultantId(int consultantId)
        {
            var availabilities = await _availabilityRepository.FilterByConsultantId(consultantId);

            return availabilities is null
                ? throw new KeyNotFoundException($"Consultant : {consultantId}")
                : availabilities.Adapt<List<AvailabilityDto>>();
        }

        public async Task UpdateAvailability(AvailabilityDto availabilityDto)
        {
            if (availabilityDto is null) throw new ArgumentNullException(nameof(availabilityDto));

            var availability = await _availabilityRepository.GetAvailability(availabilityDto.Id);
            if (availability is null) throw new KeyNotFoundException($"Availability : {availabilityDto.Id}");

            availabilityDto.Adapt(availability);
            await _availabilityRepository.UpdateAvailability(availability);
        }

        public async Task SaveAvailability(AvailabilityDto availabilityDto)
        {
            if (availabilityDto is null) throw new ArgumentNullException(nameof(availabilityDto));

            var availability = availabilityDto.Adapt<Availability>();
            await _availabilityRepository.SaveAvailability(availability);
        }

        public async Task DeleteAvailability(int id)
        {
            var availability = await _availabilityRepository.GetAvailability(id);
            if (availability is null) throw new KeyNotFoundException($"Availability : {id}");

            await _availabilityRepository.DeleteAvailability(availability);
        }
    }
}