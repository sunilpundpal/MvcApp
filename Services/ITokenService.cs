using MvcAppJwtAuth.Models;

namespace MvcAppJwtAuth.Services
{
    public interface ITokenService
    {
        string BuildToken(User user);
        bool IsValidToken(string token);
    }
}