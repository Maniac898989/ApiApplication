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
    public class TicketControllerTests
    {
        [Fact]
        public async Task CreateTicket_ReturnsTicket()
        {
            var mockItemService = new Mock<ITicketService>();

            var guid = "";
            mockItemService.Setup(service => service.CreateTicket(guid)).ReturnsAsync(new Result
            {
                IsSuccessful = true,
                Message = "Fetch successful",
                ReturnedObject = new
                {
                    id = "5a4d62ce-d33f-435c-8f25-0a4746b2e1bd",
                    showtimeId = 4,
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
                    paid = false,
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
            var controller = new TicketController(mockItemService.Object);
            var result = await controller.CreateTicket(guid);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var item = Assert.IsType<Result>(okResult.Value);

            Assert.True(item.IsSuccessful);
        }

        [Fact]
        public async Task ConfirmPayment_ReturnsTicketStatus()
        {
            var mockItemService = new Mock<ITicketService>();

            var guid = "";
            mockItemService.Setup(service => service.Confirmpayment(guid)).ReturnsAsync(new Result
            {
                IsSuccessful = true,
                Message = "Payment successful successful",
                ReturnedObject = new
                {
                    id = "5a4d62ce-d33f-435c-8f25-0a4746b2e1bd",
                    showtimeId = 4,
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
                    paid = false,
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
            var controller = new TicketController(mockItemService.Object);
            var result = await controller.ConfirmPayments(guid);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var item = Assert.IsType<Result>(okResult.Value);

            Assert.True(item.IsSuccessful);
        }
    }
}
