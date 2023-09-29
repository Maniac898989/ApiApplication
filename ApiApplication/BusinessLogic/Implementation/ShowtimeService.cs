using ApiApplication.BusinessLogic.Interfaces;
using ApiApplication.Database.Entities;
using ApiApplication.Database.Repositories.Abstractions;
using ApiApplication.Models;
using ApiApplication.Services;
using Serilog;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ApiApplication.BusinessLogic.Implementation
{
    public class ShowtimeService : IShowtimeService
    {
        private readonly IShowtimesRepository _showtimesRepository;
        private readonly IAuditoriumsRepository _auditoriumsRepository;
        private readonly IMovieClient _movieService;
        private readonly IAuditoriumService _auditoriumService;

        public ShowtimeService(IShowtimesRepository showtimesRepository, IAuditoriumsRepository auditoriumsRepository, IMovieClient movieService, IAuditoriumService auditoriumService)
        {
            _showtimesRepository = showtimesRepository;
            _auditoriumsRepository = auditoriumsRepository;
            _movieService = movieService;
            _auditoriumService = auditoriumService;
        }

        public async Task<Result> CreateShowTime(Showtime showtime)
        {

            if (showtime.ShowDate < DateTime.Today)
            {
                return new Result
                {
                    IsSuccessful = false,
                    Message = "Please select a future date"
                };
            }

            Result res = new Result();
            try
            {
                if (showtime != null)
                {
                    //call movie api to fetch movie
                    var movie = await _movieService.GetMovieByID(showtime.MovieID);
                    var auditorium = await _auditoriumService.GetSeatsByAuditoriumID(showtime.AuditoriumID);

                    if (movie.IsSuccessful && auditorium.IsSuccessful)
                    {
                        var castedObject = (MoviesResponseObject)movie.ReturnedObject;

                        var showTime = new ShowtimeEntity
                        {
                            AuditoriumId = showtime.AuditoriumID,
                            SessionDate = showtime.ShowDate,
                            Movie = new MovieEntity
                            {
                                ImdbRating = castedObject.imDbRating,
                                Image = castedObject.image,
                                Crew = castedObject.crew,
                                Title = castedObject.title,
                                Showtimes = new List<ShowtimeEntity>()
                                {
                                    new ShowtimeEntity
                                    {
                                        AuditoriumId = showtime.AuditoriumID,
                                        SessionTime = TimeSpan.Parse(showtime.ShowTime),
                                        SessionDate = showtime.ShowDate
                                    }
                                },
                                ReleaseDate = new DateTime(Convert.ToInt32(castedObject.year), 1, 1)
                            }
                        };

                        var createdShowtime = await _showtimesRepository.CreateShowtime(showTime, default(CancellationToken));
                        
                        if (createdShowtime != null)
                        {
                            res.Message = "Showtime successfully created";
                            res.IsSuccessful = true;
                            res.ReturnedObject = new  
                            {
                                Auditorium = createdShowtime.AuditoriumId,
                                ShowtimeId = createdShowtime.Id.ToString(), 
                                Movie = createdShowtime.Movie.Title,
                                ImdbRating = createdShowtime.Movie.ImdbRating,
                                Crew = createdShowtime.Movie.Crew,
                                Image = createdShowtime.Movie.Image,
                                ReleaseDate = new DateTime(Convert.ToInt32(castedObject.year), 1, 1),
                                ShowTime = showtime.ShowTime, 
                                ShowDate = showtime.ShowDate.ToString("dd-MMM-yyyy") 
                            };
                            return res;
                        }
                        else return new Result { IsSuccessful = false, Message = "Movie could not be created" };
                    }
                    else
                    {

                        res.IsSuccessful = false;
                        res.Message = "Invalid movie or Auditorium";

                    }

                }
                return new Result { IsSuccessful = false, Message = "Please provide a valid object" };
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message.ToString());
            }

            return res;
        }

        public async Task<Result> GetAllMovies()
        {
            var allMovies = await _showtimesRepository.GetAllAsync(x => x.Movie != null, default(CancellationToken));

            if (allMovies.Count() > 0)
            {
                var movies = allMovies.Select(x => new
                {
                    Id = x.Id,
                    movie = new
                    {
                        x.Movie.Id,
                        x.Movie.Title,
                        x.Movie.ImdbId,
                        x.Movie.Stars,
                        x.Movie.ReleaseDate,
                        Showtimes = x.Movie.Showtimes.Select(x => new
                        {
                            x.Id,
                            x.AuditoriumId,
                            SessionDate = x.SessionDate.ToString("dd-MMM-yyyy"),
                            SessionTime = DateTime.Parse(x.SessionTime.ToString()).ToString("hh:mm tt", CultureInfo.InvariantCulture)
                        }).ToList()
                    }

                }).ToList();
                return new Result
                {
                    IsSuccessful = true,
                    Message = "Movie fetch successful",
                    ReturnedObject = movies
                };
            }
            return new Result
            {

                IsSuccessful = false
            };

        }

        public async Task<Result> GetmoviesByID(int id)
        {
            if (id <= 0)
            {
                return new Result
                {
                    IsSuccessful = false,
                    Message = "Please provide a valid movie ID"
                };
            }

            Result res = new Result();
            try
            {
                var showtime = await _showtimesRepository.GetWithMoviesByIdAsync(id, default(CancellationToken));

                if (showtime != null)
                {
                    var movies = new
                    {
                        Id = showtime.Id,
                        movie = new
                        {
                            showtime.Movie.Id,
                            showtime.Movie.Title,
                            showtime.Movie.ImdbId,
                            showtime.Movie.Stars,
                            showtime.Movie.ReleaseDate,
                            Showtimes = showtime.Movie.Showtimes.Select(x => new
                            {
                                x.Id,
                                x.AuditoriumId,
                                SessionDate = x.SessionDate.ToString("dd-MMM-yyyy"),
                                SessionTime = DateTime.Parse(x.SessionTime.ToString()).ToString("hh:mm tt", CultureInfo.InvariantCulture)
                            }).ToList()
                        }
                    };

                    return new Result
                    {
                        IsSuccessful = true,
                        Message = "Movie fetch successful",
                        ReturnedObject = movies
                    };
                }
                else
                {
                    res.IsSuccessful = false;
                    res.Message = "No movie with that ID";
                }
                
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message.ToString());
            }
            

            return res;
        }

        public async Task<Result> GetmoviesByTicketNo(int id)
        {
            if (id <= 0)
            {
                return new Result
                {
                    IsSuccessful = false,
                    Message = "Please provide a valid movie ticket number"
                };
            }

            Result res = new Result();

            var movie = await _showtimesRepository.GetWithTicketsByIdAsync(id, default(CancellationToken));
            if (movie != null)
            {
                res.IsSuccessful = true;
                res.ReturnedObject = movie;
            }
            else
            {
                res.IsSuccessful = false;
                res.Message = "Sorry! no movie found for this Ticket ID";
            }

            return res;
        }
    }
}
