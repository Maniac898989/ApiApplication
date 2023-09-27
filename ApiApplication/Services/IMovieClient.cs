using ApiApplication.Models;
using System.Threading.Tasks;

namespace ApiApplication.Services
{
    public interface IMovieClient
    {
        Task<Result> GetMovieByID(string id);
    }
}
