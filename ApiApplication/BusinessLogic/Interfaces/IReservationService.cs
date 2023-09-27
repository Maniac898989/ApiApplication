using ApiApplication.Models;
using System.Threading.Tasks;

namespace ApiApplication.BusinessLogic.Interfaces
{
    public interface IReservationService
    {
        Task<Result> ReserveSeats(SeatReservationRequest model);
        Task<Result> GetAllReservations();
        Task<Result> GetReservationByGUID(string guid);
    }
}
