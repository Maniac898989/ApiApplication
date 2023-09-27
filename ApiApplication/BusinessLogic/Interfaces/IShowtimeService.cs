using ApiApplication.Models;
using System.Threading.Tasks;

namespace ApiApplication.BusinessLogic.Implementation
{
    public interface IShowtimeService
    {
        Task<Result> CreateShowTime(Showtime showtime);
        Task<Result> GetAllMovies();
        Task<Result> GetmoviesByID(int id);
        Task<Result> GetmoviesByTicketNo(int id);


    }
}
