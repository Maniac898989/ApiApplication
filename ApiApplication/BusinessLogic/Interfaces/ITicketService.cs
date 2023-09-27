using ApiApplication.Database.Entities;
using ApiApplication.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ApiApplication.BusinessLogic.Interfaces
{
    public interface ITicketService
    {
        Task<Result> CreateTicket(string guid);
        Task<Result> Confirmpayment(string guid);
    }
}
