using ApiApplication.Models;

namespace ApiApplication.BusinessLogic.Interfaces
{
    public interface IUserService
    {
        AuthToken CheckUser(string UserId);
    }
}
