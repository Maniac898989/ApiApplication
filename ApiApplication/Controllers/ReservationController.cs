using ApiApplication.BusinessLogic.Interfaces;
using ApiApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ApiApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationController : Controller
    {
        private readonly IReservationService _reservation;
        public ReservationController(IReservationService reservation)
        {
            _reservation = reservation;
        }

        Stopwatch stopwatch = new Stopwatch();

        [HttpPost("reserve-seats")]
        public async Task<IActionResult> ReserveSeats(SeatReservationRequest model)
        {
            stopwatch.Start();

            var result = await _reservation.ReserveSeats(model);

            stopwatch.Stop();
            var elapsed = stopwatch.Elapsed.TotalSeconds;
            Log.Information($"The ReserveSeat endpoint took ====> {elapsed} seconds");

            if (result.IsSuccessful) return Ok(result);
            else return BadRequest(result);
        }

        [HttpGet("get-all-reservations")]
        public async Task<IActionResult> GetAllReservations ()
        {
            stopwatch.Start();

            var result = await _reservation.GetAllReservations();

            stopwatch.Stop();
            var elapsed = stopwatch.Elapsed.TotalSeconds;
            Log.Information($"The GetAllReservation endpoint took ====> {elapsed} seconds");

            if (result.IsSuccessful) return Ok(result);
            else return BadRequest(result);
        }

        [HttpGet("get-reserved-seats/{guid}")]
        public async Task<IActionResult> GetReservedSeats(string guid)
        {
            stopwatch.Start();

            var result = await _reservation.GetReservationByGUID(guid);

            stopwatch.Stop();
            var elapsed = stopwatch.Elapsed.TotalSeconds;
            Log.Information($"The GetReservedSeat endpoint took ====> {elapsed} seconds");

            if (result.IsSuccessful) return Ok(result);
            else return BadRequest(result);
        }
    }
}
