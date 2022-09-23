using MvcAppJwtAuth.Models;

namespace MvcAppJwtAuth.Services
{

    public interface IUserService
    {
        User Authenticate(string username, string password);
    }
}