using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CalifornianHealth.Booking.Controllers;
using CalifornianHealth.Booking.Data.DTO;
using CalifornianHealth.Booking.Features.Commands.Booking;
using CalifornianHealth.Booking.Features.Queries.Booking;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Xunit;

namespace CalifornianHealth.Tests.Controllers
{
    public class BookingControllerTest
    {
        private readonly BookingDto _bookingDto;
        private readonly IMediator _mediator;
        private readonly BookingController _sut;

        public BookingControllerTest()
        {
            _mediator = Substitute.For<IMediator>();
            _sut = new BookingController(_mediator);
            _bookingDto = new BookingDto {Id = 99, ConsultantId = 999};
        }

        [Fact]
        public async Task GetAllAsync()
        {
            _mediator.Send(Arg.Any<GetAll.Query>())
                .Returns(new GetAll.Response(new List<BookingDto>()));

            var actionResult = await _sut.GetAllAsync();
            if (actionResult.Result is OkObjectResult result)
            {
                result.StatusCode.Should().Be(200);
                if (result.Value is List<BookingDto> value) value.Should().BeEmpty();
            }
        }

        [Fact]
        public async Task GetByIdAsync()
        {
            _mediator.Send(Arg.Any<Get.Query>())
                .Returns(new Get.Response(_bookingDto));

            var actionResult = await _sut.GetByIdAsync(new Random().Next());

            if (actionResult.Result is OkObjectResult result)
            {
                result.StatusCode.Should().Be(200);
                if (result.Value is BookingDto value)
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
                .Returns(new GetByConsultantId.Response(new List<BookingDto>()));

            var actionResult = await _sut.GetByConsultantIdAsync(new Random().Next());

            if (actionResult.Result is OkObjectResult result)
            {
                result.StatusCode.Should().Be(200);
                if (result.Value is List<BookingDto> value) value.Should().BeEmpty();
            }
        }

        [Fact]
        public async Task PostAsync()
        {
            _mediator.Send(Arg.Any<PostBooking.Command>())
                .Returns(new BookingDto());

            var actionResult = await _sut.PostAsync(new PostBooking.Command(_bookingDto));

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
            _mediator.Send(Arg.Any<PutBooking.Command>())
                .Returns(_bookingDto);

            var actionResult = await _sut.PutAsync(_bookingDto);

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

            _mediator.Send(Arg.Any<DeleteBooking.Command>())
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