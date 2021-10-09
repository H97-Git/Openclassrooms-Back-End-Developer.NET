(function ($) {
    const month = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];
    const today = moment();
    let monthOffset = 0;

    window.LoadCalendarDate = () => {

        let startDay = today.clone();
        let endDay = today.clone();

        if (monthOffset >= 0) {
            startDay = startDay.add(monthOffset, "month");
            endDay = endDay.add(monthOffset, "month");
            let indexMonth = today.month() + monthOffset;
            // indexMonth = indexMonth > 12 ? indexMonth - 12 : indexMonth
            document.querySelector(".calendar-container__title").innerText = month[indexMonth];
        } else {
            startDay = startDay.subtract(1, "month");
            endDay = endDay.subtract(1, "month");
            document.querySelector(".calendar-container__title").innerText = month[today.month() - 1];
        }

        startDay = startDay.startOf("month").startOf("week").subtract(1, "day");
        endDay = endDay.endOf("month").endOf("week");

        LoadCalendar(startDay, endDay);
    }

    window.LoadCalendar = (startDay, endDay) => {
        const calendar = [];

        while (startDay.isBefore(endDay, "day")) {
            calendar.push({
                days: Array(7).fill(0).map(() => startDay.add(1, "day").clone())
            });
        }

        const calItem = document.querySelectorAll(".calendar-table__item");
        let index = 0;
        for (let i = 0; i < calendar.length; i++) {
            for (let y = 0; y < calendar[i].days.length; y++) {
                if (index > 34)
                    return;
                const calDate = calendar[i].days[y].date();
                calItem[index].innerText = calDate;
                let todayClone = today.clone();
                // console.log("------------------- LOG -----------------")
                // console.log("today",todayClone.date());
                // console.log("date",calendar[i].days[y].date());
                // console.log("endOf",todayClone.endOf("month").date());
                // console.log("isBefore",calendar[i].days[y].isBefore(todayClone, "hour") );
                // console.log("isAfter",calendar[i].days[y].isAfter(todayClone.endOf("month"), "hour"));          
                // console.log("------------------- END -----------------")
                if (calendar[i].days[y].isBefore(todayClone, "day")) {
                    calItem[index].parentElement.classList.add("calendar-table__inactive");
                } else {
                    calItem[index].parentElement.classList.remove("calendar-table__inactive");
                }
                index++;
            }
        }
    }

    window.LoadListener = () => {
        $(".calendar-table__item").on('click', function () {
            let availabilities = $(".events__item");
            for (const availability of availabilities) {
                let month = today.month() + monthOffset;
                let btnText = `${month + 1}/${$(this).text()}`
                let isSameDay = `${availability.getAttribute('data-month')}/${availability.getAttribute('data-day')}` === btnText;
                if (isSameDay) {
                    $(availability).show();
                } else {
                    $(availability).hide();
                }
            }
        });

        $(".calendar-container__btn").on('click', function (event) {
            if (event.currentTarget.classList.contains('calendar-container__btn--right')) {
                if ($(".calendar-container__title").text() === "December") {
                    FireNotif("Wait next year for further date.");
                    return;
                }
                monthOffset++;
            } else {
                if ($(".calendar-container__title").text() === month[today.month() - 1]) {
                    FireNotif("You can't book in the past !");
                    return;
                }
                monthOffset--;
            }
            LoadCalendarDate();
        });
    }

    window.ConfirmBooking = (model) => {
        console.log(model);
        const date = moment(model.availabilityModel.dateTime).format('ddd MMMM Do, YYYY');
        const hour = moment(model.availabilityModel.dateTime).format('HH:mm');
        const request = {
            bookingDto: {
                consultantId: model.availabilityModel.consultantId,
                availabilityId: model.availabilityModel.id,
                appointment: model.availabilityModel.dateTime
            }
        };
        console.log(request);
        Swal.fire({
            template: '#confirmBookingTemplate',
            allowOutsideClick: () => !Swal.isLoading(),
            preConfirm: () => {
                return axios.post('http://localhost:7000/Booking/add', request)
                    .then(function (response) {
                        return response;
                    })
                    .catch(function (error) {
                        Swal.showValidationMessage(
                            `Request failed: ${error}`
                        );
                    });
            }
        }).then((result) => {
            if (result.isConfirmed) {
                Swal.fire({
                    icon: 'success',
                    title: `Booked successfully !`,
                    showConfirmButton: false,
                    focusConfirm: false,
                    timer: 1500
                });
            }
        });
        SetConfirmBookingDoms(model, date, hour);
    }

    function SetConfirmBookingDoms(model, date, hour) {
        document.querySelector("#confirmBookingName").innerText =
            `${model.consultantModel.givenName} ${model.consultantModel.familyName}`;
        document.querySelector("#confirmBookingImg").src = `img/doctor${model.consultantModel.id}.jpg`;
        document.querySelector("#confirmBookingSpecialty").innerText = `${model.consultantModel.specialty}`;
        document.querySelector("#confirmBookingDate").innerText = `${date}`;
        document.querySelector("#confirmBookingHour").innerText = `${hour}`;
    }

    function FireNotif(message) {
        Swal.fire({
            icon: 'warning',
            toast: true,
            timer: 2500,
            timerProgressBar: true,
            title: message,
            position: 'top-end',
            showCloseButton: true,
            showConfirmButton: false,
            background: "#1a6470",
            customClass: {
                popup: 'calendarSwal2',
                title: 'calendarSwal2_Title',
            }
        });
    }

    setTimeout(function () {
        $('.nav a').on('click', function (e) {
            e.preventDefault();
            let href = $(this).attr('href');
            let offset = href !== '' ? $(href).offset().top : 0;
            $('html, body').animate({
                scrollTop: offset
            }, 500);
            return false;
        });

        //jQuery to collapse the navbar on scroll
        $(window).on('scroll', function () {
            // $('.nav a').blur();
            if ($('.navbar-default').offset().top > 50) {
                $('.navbar-fixed-top').addClass('top-nav-collapse');
            } else {
                $('.navbar-fixed-top').removeClass('top-nav-collapse');
            }
        });

        if ($('.navbar-default').offset().top > 50) {
            $('.navbar-fixed-top').addClass('top-nav-collapse');
        }

        $("body").show();
    }, 500);
})(jQuery);