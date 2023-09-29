using ApiApplication.BusinessLogic.Interfaces;
using ApiApplication.Database.Entities;
using ApiApplication.Database.Repositories.Abstractions;
using ApiApplication.Models;
using Serilog;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ApiApplication.BusinessLogic.Implementation
{
    public class TicketService : ITicketService
    {
        private readonly IReservationService _reservationService;
        private readonly ITicketsRepository _ticketsRepository;

        public TicketService(IReservationService reservationService, IAuditoriumService auditoriumService, ITicketsRepository ticketsRepository)
        {
            _reservationService = reservationService;
            _ticketsRepository = ticketsRepository;
        }


        public async Task<Result> CreateTicket(string guid)
        {
            if (string.IsNullOrEmpty(guid))
            {
                return new Result
                {
                    IsSuccessful = false,
                    Message = "Please enter a valid Reservation number for ticket creation"
                };
            }


            Result res = new Result();
            try
            {
                //get reservation object first
                var reservation = await _reservationService.GetReservationByGUID(guid);

                var reservationObject = (ReservationEntity)reservation.ReturnedObject;
                if (reservation != null)
                {
                    var createdTicket = await _ticketsRepository.CreateAsync(reservationObject.Showtime, reservationObject.Seats, default(CancellationToken));
                    if (createdTicket != null)
                    {
                        res.IsSuccessful = true;
                        res.ReturnedObject = createdTicket;
                    }
                    else
                    {
                        res.IsSuccessful = false;
                        res.Message = "Unable to fetch create Ticket";
                    }

                }
                else
                {
                    res.IsSuccessful = false;
                    res.Message = "Seat not found for that Id";
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message.ToString());
                res.IsSuccessful = false;
            }
            return res;
        }

        private async Task<Result> ConfirmPaymentByTicket(TicketEntity ticket)
        {
            Result res = new Result();
            try
            {
                var IsPaymentSuccessful = await _ticketsRepository.ConfirmPaymentAsync(ticket, default(CancellationToken));
                if (IsPaymentSuccessful.Paid)
                {
                    res.IsSuccessful = true;
                    res.Message = "Payment Successful";
                    res.ReturnedObject = IsPaymentSuccessful;
                }
                else
                {
                    res.IsSuccessful = false;
                    res.Message = "Payment failed";
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message.ToString());
            }

            return res;
        }

        public async Task<Result> Confirmpayment(string guid)
        {
            if (string.IsNullOrEmpty(guid))
            {
                return new Result
                {
                    IsSuccessful = false,
                    Message = "Please enter a valid Reservation number for complete your purchase"
                };
            }
            Result res = new Result();
            try
            {
                if (!string.IsNullOrEmpty(guid))
                {
                    var ticketConfirmation = await _ticketsRepository.GetAsync(Guid.Parse(guid), default(CancellationToken));

                    if (ticketConfirmation != null)
                    {
                        //check here to checkmate paid seats
                        if (ticketConfirmation.Paid == true)
                        {
                            res.Message = "Sorry you have aleady paid for this seat and cannot pay for it again";
                            res.IsSuccessful = false;
                            return res;
                        }

                        var IsPaymentSuccessful = await _ticketsRepository.ConfirmPaymentAsync(ticketConfirmation, default(CancellationToken));
                        if (IsPaymentSuccessful.Paid)
                        {
                            res.IsSuccessful = true;
                            res.Message = "Payment Successful";
                            res.ReturnedObject = ticketConfirmation;
                        }
                        else
                        {
                            res.IsSuccessful = false;
                            res.Message = "Payment failed!";
                        }
                    }
                }
                else
                {
                    res.Message = "Please provide a valid guid";
                    res.IsSuccessful = false;
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
