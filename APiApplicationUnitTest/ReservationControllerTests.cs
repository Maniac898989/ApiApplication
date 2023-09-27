using ApiApplication.BusinessLogic.Implementation;
using ApiApplication.BusinessLogic.Interfaces;
using ApiApplication.Controllers;
using ApiApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace APiApplicationUnitTest
{
    public class ReservationControllerTests
    {

        [Fact]
        public async Task CreateReservation_ReturnsReservations()
        {
            var mockItemService = new Mock<IReservationService>();

            // Arrange
            var reservationRequest = new SeatReservationRequest
            {
                NoOfReservationSeat = 1,
                movieID = 1,
                ReservationDate = DateTime.Now,
                ReservationTime = "12:45"
            };

            mockItemService.Setup(service => service.ReserveSeats(reservationRequest)).ReturnsAsync(new Result
            {
                IsSuccessful = true,
                Message = "Reservation completed successfully",
                ReturnedObject = new
                {
                    guid = "e855694b-0a79-443d-903b-808214ef44df",
                    numberOfSeats = 1,
                    auditorium = 1,
                    movieTitle = "Inception",
                    seats = new
                    {
                        row = 1,
                        seatNumber = 1,
                        auditorium = 1,
                        isReserved = true
                    },
                    showtime = new
                    {
                        auditoriumId = 1,
                        movie = new
                        {
                            title = "Inception"
                        },
                        sessionDate = DateTime.Now,
                        sessionTime = "12:00"
                    }

                }
            });


            // Act
            var controller = new ReservationController(mockItemService.Object);
            var result = await controller.ReserveSeats(reservationRequest);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var item = Assert.IsType<Result>(okResult.Value);

            Assert.True(item.IsSuccessful);
        }

        [Fact]
        public async Task GetAllReservations_ReturnsReservations()
        {
            var mockItemService = new Mock<IReservationService>();


            mockItemService.Setup(service => service.GetAllReservations()).ReturnsAsync(new Result
            {
                IsSuccessful = true,
                Message = "Reservation fetch successful",
                ReturnedObject = new
                {
                    guid = "e855694b-0a79-443d-903b-808214ef44df",
                    numberOfSeats = 1,
                    auditorium = 1,
                    movieTitle = "Inception",
                    seats = new
                    {
                        row = 1,
                        seatNumber = 1,
                        auditorium = 1,
                        isReserved = true
                    },
                    showtime = new
                    {
                        auditoriumId = 1,
                        movie = new
                        {
                            title = "Inception"
                        },
                        sessionDate = DateTime.Now,
                        sessionTime = "12:00"
                    },
                    createdTime = DateTime.Now

                }
            });


            // Act
            var controller = new ReservationController(mockItemService.Object);
            var result = await controller.GetAllReservations();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var item = Assert.IsType<Result>(okResult.Value);

            Assert.True(item.IsSuccessful);
        }

        [Fact]
        public async Task GetReservationById_ReturnsReservations()
        {
            var mockItemService = new Mock<IReservationService>();

            var guid = "";
            mockItemService.Setup(service => service.GetReservationByGUID(guid)).ReturnsAsync(new Result
            {
                IsSuccessful = true,
                Message = "Fetch successful",
                ReturnedObject = new
                {
                    id = 1,
                    guid = "e855694b-0a79-443d-903b-808214ef44df",
                    numberOfSeats = 1,
                    auditorium = 1,
                    movieTitle = "Inception",
                    seats = new
                    {
                        row = 1,
                        seatNumber = 1,
                        auditorium = 1,
                        isReserved = true
                    },
                    showtime = new
                    {
                        auditoriumId = 1,
                        movie = new
                        {
                            title = "Inception"
                        },
                        sessionDate = DateTime.Now,
                        sessionTime = "12:00"
                    },
                    createdTime = DateTime.Now

                }
            });


            // Act
            var controller = new ReservationController(mockItemService.Object);
            var result = await controller.GetReservedSeats(guid);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var item = Assert.IsType<Result>(okResult.Value);

            Assert.True(item.IsSuccessful);
        }
    }
}
