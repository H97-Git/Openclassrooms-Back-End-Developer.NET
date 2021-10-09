using System;
using System.Threading;
using System.Threading.Tasks;
using CalifornianHealth.Booking.Infrastructure.Services.Interface;
using EasyNetQ;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CalifornianHealth.Booking.BackgroundServices
{
    public record BookingRequest(int AvailabilityId);

    public class BookingEventHandler : BackgroundService
    {
        private readonly IAvailabilityService _availabilityService;
        private readonly IBus _bus;

        public BookingEventHandler(IBus bus, IServiceProvider serviceProvider)
        {
            _bus = bus;
            _availabilityService =
                serviceProvider.CreateScope().ServiceProvider.GetRequiredService<IAvailabilityService>();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _bus.Rpc.RespondAsync<BookingRequest, BookingResponse>(
                request => CheckAvailability(request.AvailabilityId), stoppingToken);
        }

        private async Task<BookingResponse> CheckAvailability(int availabilityId)
        {
            try
            {
                var availability = await _availabilityService.GetAvailability(availabilityId);
                return new BookingResponse(availability is not null);
            }
            catch (Exception ex)
            {
                return new BookingResponse(false, ex);
            }
        }
    }

    public record BookingResponse(bool IsBookable, Exception Error = null);
}