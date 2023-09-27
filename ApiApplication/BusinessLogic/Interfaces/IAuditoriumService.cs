using ApiApplication.Models;
using System.Threading.Tasks;

namespace ApiApplication.BusinessLogic.Interfaces
{
    public interface IAuditoriumService
    {
        Task<Result> GetSeatsByAuditoriumID(int id);
    }
}
