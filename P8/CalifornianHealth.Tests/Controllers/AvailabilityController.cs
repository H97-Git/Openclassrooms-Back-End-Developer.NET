using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CalifornianHealth.Booking.Controllers;
using CalifornianHealth.Booking.Data.DTO;
using CalifornianHealth.Booking.Features.Commands.Availability;
using CalifornianHealth.Booking.Features.Queries.Availability;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Xunit;

namespace CalifornianHealth.Tests.Controllers
{
    public class AvailabilityControllerTest
    {
        private readonly AvailabilityDto _availabilityDto;
        private readonly IMediator _mediator;
        private readonly AvailabilityController _sut;

        public AvailabilityControllerTest()
        {
            _mediator = Substitute.For<IMediator>();
            _sut = new AvailabilityController(_mediator);
            _availabilityDto = new AvailabilityDto {Id = 99, ConsultantId = 999};
        }

        [Fact]
        public async Task GetAllAsync()
        {
            _mediator.Send(Arg.Any<GetAll.Query>())
                .Returns(new GetAll.Response(new List<AvailabilityDto>()));

            var actionResult = await _sut.GetAllAsync();
            if (actionResult.Result is OkObjectResult result)
            {
                result.StatusCode.Should().Be(200);
                if (result.Value is List<AvailabilityDto> value) value.Should().BeEmpty();
            }
        }

        [Fact]
        public async Task GetByIdAsync()
        {
            _mediator.Send(Arg.Any<Get.Query>())
                .Returns(new Get.Response(_availabilityDto));

            var actionResult = await _sut.GetByIdAsync(new Random().Next());

            if (actionResult.Result is OkObjectResult result)
            {
                result.StatusCode.Should().Be(200);
                if (result.Value is AvailabilityDto value)
                {
                    value.Id.Should().Be(99);
                    value.ConsultantId.Should().Be(999);
                }
            }
        }

        [Fact]
        public async Task GetByConsultantIdAsync()
        {
            _mediator.Send(Arg.Any<GetByConsultantId.Query>())
                .Returns(new GetByConsultantId.Response(new List<AvailabilityDto>()));

            var actionResult = await _sut.GetByConsultantIdAsync(new Random().Next());

            if (actionResult.Result is OkObjectResult result)
            {
                result.StatusCode.Should().Be(200);
                if (result.Value is List<AvailabilityDto> value) value.Should().BeEmpty();
            }
        }

        [Fact]
        public async Task PostAsync()
        {
            _mediator.Send(Arg.Any<PostAvailability.Command>())
                .Returns(new AvailabilityDto());

            var actionResult = await _sut.PostAsync(new PostAvailability.Command(_availabilityDto));

            if (actionResult.Result is OkObjectResult result)
            {
                result.StatusCode.Should().Be(200);
                if (result.Value is AvailabilityDto value)
                {
                    value.Id.Should().Be(99);
                    value.ConsultantId.Should().Be(999);
                }
            }
        }

        [Fact]
        public async Task PutAsync()
        {
            _mediator.Send(Arg.Any<PutAvailability.Command>())
                .Returns(_availabilityDto);

            var actionResult = await _sut.PutAsync(_availabilityDto);

            if (actionResult.Result is OkObjectResult result)
            {
                result.StatusCode.Should().Be(200);
                if (result.Value is AvailabilityDto value)
                {
                    value.Id.Should().Be(99);
                    value.ConsultantId.Should().Be(999);
                }
            }
        }

        [Fact]
        public async Task DeleteAsync()
        {
            var r = new Random().Next();

            _mediator.Send(Arg.Any<DeleteAvailability.Command>())
                .Returns(r);

            var actionResult = await _sut.DeleteAsync(r);

            if (actionResult.Result is OkObjectResult result)
            {
                result.StatusCode.Should().Be(200);
                if (result.Value is int value) value.Should().Be(r);
            }
        }
    }
}