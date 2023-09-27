using ApiApplication.BusinessLogic.Implementation;
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
    public class ShowtimeControllerTests
    {
        [Fact]
        public async Task CreateShowtime_ReturnsShowtime()
        {
            var mockItemService = new Mock<IShowtimeService>();

            // Arrange
            var showtime = new Showtime
            {
                AuditoriumID = 1,
                MovieID = "ID-test1",
                ShowDate = DateTime.Now,
                ShowTime = "12:45"
            };

            mockItemService.Setup(service => service.CreateShowTime(showtime)).ReturnsAsync(new Result
            {
                IsSuccessful = true,
                Message = "Showtime successfully created",
                ReturnedObject = new
                {
                    showtimeId = 2,
                    auditorium = 1,
                    movie = "The mock movie",
                    showTime = "12:45",
                    showDate = "26-Sep-2023"
                }
            });


            // Act
            var controller = new ShowTimeController(mockItemService.Object);
            var result = await controller.CreateShowTime(showtime);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var item = Assert.IsType<Result>(okResult.Value);

            Assert.True(item.IsSuccessful);
        }

        [Fact]
        public async Task GetAllMovies_ReturnsAllMovies()
        {
            var mockItemService = new Mock<IShowtimeService>();


            mockItemService.Setup(service => service.GetAllMovies()).ReturnsAsync(new Result
            {
                IsSuccessful = true,
                Message = "Movie fetch successful",
                ReturnedObject = new
                {
                    id = 2,
                    movie = new
                    {
                        id = 1,
                        title = "Inception",
                        imdbId = "tt1375666",
                        stars = "Leonardo DiCaprio, Joseph Gordon-Levitt, Ellen Page, Ken Watanabe",
                        releaseDate = "2010-01-14T00:00:00",
                    },
                    showtimes = new
                    {
                        id = 1,
                        auditoriumId = 1,
                        sessionDate = "01-Jan-2023",
                        sessionTime = "12:30 PM"
                    }
                }
            });


            // Act
            var controller = new ShowTimeController(mockItemService.Object);
            var result = await controller.GetAllMovies();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var item = Assert.IsType<Result>(okResult.Value);

            Assert.True(item.IsSuccessful);
        }

        [Fact]
        public async Task GetMoviesById_ReturnsSpecificMovies()
        {
            var mockItemService = new Mock<IShowtimeService>();

            var id = 1;
            mockItemService.Setup(service => service.GetmoviesByID(id)).ReturnsAsync(new Result
            {
                IsSuccessful = true,
                Message = "Movie fetch successful",
                ReturnedObject = new
                {
                    id = 1,
                    movie = new
                    {
                        id = 1,
                        title = "Inception",
                        imdbId = "tt1375666",
                        stars = "Leonardo DiCaprio, Joseph Gordon-Levitt, Ellen Page, Ken Watanabe",
                        releaseDate = "2010-01-14T00:00:00",
                    },
                    showtimes = new
                    {
                        id = 1,
                        auditoriumId = 1,
                        sessionDate = "01-Jan-2023",
                        sessionTime = "12:30 PM"
                    }
                }
            });


            // Act
            var controller = new ShowTimeController(mockItemService.Object);
            var result = await controller.GetMoviesByID(id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var item = Assert.IsType<Result>(okResult.Value);

            Assert.True(item.IsSuccessful);
        }
    }
}
