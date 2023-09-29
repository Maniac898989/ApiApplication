using ApiApplication.BusinessLogic.Implementation;
using ApiApplication.BusinessLogic.Interfaces;
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
    public class TicketController : Controller
    {
        private readonly ITicketService _ticketService;

        public TicketController(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }

        Stopwatch stopwatch = new Stopwatch();

        [HttpPost("create-ticket")]
        public async Task<IActionResult> CreateTicket(string guid)
        {
            stopwatch.Start();

            var result = await _ticketService.CreateTicket(guid);

            stopwatch.Stop();
            var elapsed = stopwatch.Elapsed.TotalSeconds;
            Log.Information($"The CreateTicket endpoint took ====> {elapsed} seconds");

            if (result.IsSuccessful) return Ok(result);
            else return BadRequest(result);
        }

        [HttpPost("confirm-payment")]
        public async Task<IActionResult> ConfirmPayments(string guid)
        {
            stopwatch.Start();

            var result = await _ticketService.Confirmpayment(guid);

            stopwatch.Stop();
            var elapsed = stopwatch.Elapsed.TotalSeconds;
            Log.Information($"The CreateTicket endpoint took ====> {elapsed} seconds");

            if (result.IsSuccessful) return Ok(result);
            else return BadRequest(result);
        }
    }
}
