using ApiApplication.BusinessLogic.Implementation;
using ApiApplication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ApiApplication.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ShowTimeController : Controller
    {
        private readonly IShowtimeService _showtimeService;
        public ShowTimeController(IShowtimeService showtimeService)
        {
            _showtimeService = showtimeService;
        }

        Stopwatch stopwatch = new Stopwatch();

        [HttpPost("create-showtime")]
        public async Task<IActionResult> CreateShowTime(Showtime model)
        {
            stopwatch.Start();

            var result = await _showtimeService.CreateShowTime(model);

            stopwatch.Stop();
            var elapsed = stopwatch.Elapsed.TotalSeconds;
            Log.Information($"The CreateShowTime endpoint took ====> {elapsed} seconds");

            if (result.IsSuccessful) return Ok(result);  
            else return BadRequest(result);
        }

        [HttpGet("get-all-showtimes")]
        public async Task<IActionResult> GetAllMovies()
        {
            stopwatch.Start();

            var result = await _showtimeService.GetAllMovies();

            stopwatch.Stop();
            var elapsed = stopwatch.Elapsed.TotalSeconds;
            Log.Information($"The GetAllMoviesShowtime endpoint took ====> {elapsed} seconds");

            if (result.IsSuccessful) return Ok(result);
            else return BadRequest(result);
        }

        [HttpGet("get-showtime-by-movieId/{id}")]
        public async Task<IActionResult> GetMoviesByID(int id)
        {
            stopwatch.Start();

            var result = await _showtimeService.GetmoviesByID(id);

            stopwatch.Stop();
            var elapsed = stopwatch.Elapsed.TotalSeconds;
            Log.Information($"The GetShowTimeByMovie endpoint took ====> {elapsed} seconds");

            if (result.IsSuccessful) return Ok(result);
            else return BadRequest(result);
        }
    }
}
