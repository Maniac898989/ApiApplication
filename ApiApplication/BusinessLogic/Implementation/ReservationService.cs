using ApiApplication.BusinessLogic.Interfaces;
using ApiApplication.Database.Repositories.Abstractions;
using ApiApplication.Models;
using ApiApplication.Services;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using ApiApplication.Database.Entities;
using System;
using Serilog;
using System.Globalization;

namespace ApiApplication.BusinessLogic.Implementation
{
    public class ReservationService : IReservationService
    {
        private readonly IAuditoriumsRepository _auditoriumsRepository;
        private readonly IMovieClient _movieService;
        private readonly IShowtimesRepository _showtimesRepository;
        private readonly IReservationRepository _reservationRepository;
        private readonly ITicketsRepository _ticketsRepository;


        public ReservationService(IAuditoriumsRepository auditoriumsRepository, IMovieClient movieService, IShowtimesRepository showtimesRepository, IReservationRepository reservationRepository, ITicketsRepository ticketsRepository)
        {
            _auditoriumsRepository = auditoriumsRepository;
            _movieService = movieService;
            _showtimesRepository = showtimesRepository;
            _reservationRepository = reservationRepository;
            _ticketsRepository = ticketsRepository;
        }

        public async Task<Result> ReserveSeats(SeatReservationRequest model)
        {
            if(model.NoOfReservationSeat <= 0)
            {
                return new Result
                {
                   IsSuccessful = false,
                   Message = "Please enter a seat reservation greater than 0"
                };
            }

            if (model.ReservationDate < DateTime.Today)
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
                //Get movie first
                var movie = await _showtimesRepository.GetWithMoviesByIdAsync(model.movieID, default(CancellationToken));

                if (movie != null)
                {
                    var availableSeats = await _auditoriumsRepository.GetAsync(movie.AuditoriumId, default(CancellationToken));

                    if (availableSeats != null)
                    {
                        var noOfAvailableSeats = availableSeats.Seats.ToList();

                        if (noOfAvailableSeats.Count >= model.NoOfReservationSeat)
                        {
                            //Get number of reserved Seats
                            //Using C# .Take() Method returns a specified number of contiguous elements from the start of a sequence.
                            var reservedSeats = noOfAvailableSeats.Take(model.NoOfReservationSeat).ToList();

                            //use this reserve seats to check the ticket table to confirm if it has been paid for
                            var isSitPaidfor = await _ticketsRepository.GetAllPaidTicketsWithSeatAsync(default(CancellationToken));
                            if (isSitPaidfor != null)
                            {
                                var hasMatch = isSitPaidfor.Select(x => x.Seats.Intersect(reservedSeats)).Any();
                                if (hasMatch)
                                {
                                    return new Result { IsSuccessful = false, Message = "Sorry! This seat has been paid for, please try again later" };
                                }
                            }

                            //check if its within the 10mins range
                            var IsDuplicateBooking = await BookingExceedsTenMins(reservedSeats);
                            if (IsDuplicateBooking)
                            {
                                return new Result { IsSuccessful = false, Message = "You cannot book the same seat in less than 10 mins" };
                            }


                            noOfAvailableSeats.RemoveRange(0, model.NoOfReservationSeat);
                            //Get number of available seats
                            var remainingSeats = noOfAvailableSeats.Count;


                            var GUID = Guid.NewGuid().ToString();
                            var reservation = new ReservationEntity
                            {
                                GUID = GUID,
                                NumberOfSeats = reservedSeats.Count(),
                                AuditoriumID = movie.AuditoriumId,
                                MovieTitle = movie.Movie.Title,
                                Seats = reservedSeats,
                                Showtime = new ShowtimeEntity
                                {
                                    AuditoriumId = movie.AuditoriumId,
                                    Movie = new MovieEntity
                                    {
                                        Title = movie.Movie.Title
                                    },
                                    SessionDate = model.ReservationDate,
                                    SessionTime = TimeSpan.Parse(model.ReservationTime)
                                }
                            };

                            foreach (var seat in reservedSeats)
                            {
                                seat.IsReserved = true;
                            }

                            //add reservation to DB
                            var i = await _reservationRepository.CreateReservation(reservation, default(CancellationToken));
                            if (i != null)
                            {
                                var reservationResp = new
                                {
                                    GUID = GUID,
                                    NumberOfSeats = reservedSeats.Count(),
                                    Auditorium = movie.AuditoriumId,
                                    MovieTitle = movie.Movie.Title,
                                    Seats = reservedSeats.Select(x => new { row = x.Row, seatNumber = x.SeatNumber, auditorium = x.AuditoriumId, isReserved = x.IsReserved }),
                                    Showtime = new
                                    {
                                        AuditoriumId = movie.AuditoriumId,
                                        Movie = new
                                        {
                                            Title = movie.Movie.Title
                                        },
                                        SessionDate = model.ReservationDate,
                                        SessionTime = TimeSpan.Parse(model.ReservationTime)
                                    }
                                };



                                res.IsSuccessful = true;
                                res.ReturnedObject = reservationResp;
                                res.Message = "Reservation completed successfully";
                                Console.WriteLine(noOfAvailableSeats);
                            }
                            else
                            {
                                res.IsSuccessful = false;
                                res.Message = "Could not create reservarion";
                            }

                        }
                        else
                        {
                            res.IsSuccessful = false;
                            res.Message = "There arent anymore seats for reservation. Pleasr try again later";
                        }
                    }
                }
                else
                {
                    res.IsSuccessful = false;
                    res.Message = "Movie not found with this ID";
                }

                return res;
            }
            catch (Exception ex)
            {
                res.IsSuccessful = false;
                Log.Error(ex, ex.Message.ToString());
            }
            return res;
        }

        private async Task<bool> BookingExceedsTenMins(IEnumerable<SeatEntity> seats)
        {
            var currentTime = DateTime.Now;
            var timeLimit = TimeSpan.FromMinutes(10);
            var resp = await _reservationRepository.GetAllReservationsAsync(default(CancellationToken));

            if (resp.Count() > 0)
            {
                foreach (var item in resp)
                {
                    var validTime = currentTime - item.CreatedTime;

                    //check for existing
                    var hasMatch = resp.Select(x => x.Seats.Intersect(seats)).Any();
                    if (validTime <= timeLimit && hasMatch) return true;
                }

            }
            return false;
        }


        public async Task<Result> GetAllReservations()
        {
            Result res = new Result();

            try
            {
                var allReservations = await _reservationRepository.GetAllReservationsAsync(default(CancellationToken));
                if (allReservations.Count() > 0)
                {
                    res.Message = "Reservation fetch successful";
                    res.IsSuccessful = true;
                    res.ReturnedObject = allReservations;

                }
                else
                {
                    res.IsSuccessful = false;
                    res.Message = "There are no existing reservations";
                }
            }
            catch (Exception ex)
            {
                res.IsSuccessful = false;
                Log.Error(ex, ex.Message.ToString());
            }

            return res;

        }

        public async Task<Result> GetReservationByGUID(string guid)
        {
            if (string.IsNullOrEmpty(guid))
            {
                return new Result
                {
                    IsSuccessful = false,
                    Message = "Please provide a valid Reservation Reference"
                };
            }

            Result res = new Result();
            try
            {

                var movieReservered = await _reservationRepository.GetReservationByGUIDAsync(guid, default(CancellationToken));
                if (movieReservered != null)
                {
                    res.IsSuccessful = true;
                    res.Message = "Fetch successful";
                    res.ReturnedObject = movieReservered;
                }
                else
                {
                    res.IsSuccessful = false;
                    res.Message = "Sorry! there is no reservation with this Guid";
                }
            }
            catch (Exception ex)
            {
                res.IsSuccessful = false;
                Log.Error(ex, ex.Message.ToString());
            }
            return res;
        }
    }
}
