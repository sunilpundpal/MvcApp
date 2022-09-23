using MvcAppJwtAuth.Core.Constants;
using MvcAppJwtAuth.Models;

namespace MvcAppJwtAuth.Services
{
    public class UserService : IUserService
    {
        private readonly List<User> users = new List<User>();
        public UserService()
        {
            users = new List<User>
        {
            new User { Id=1, Name= "Sunil", Email= "sunil.pundpal@gmail.com", Role = UserRolesConstant.Admin , Password="1234"},
            new User { Id=1, Name= "Sonam", Email= "sonam.pundpal@gmail.com",Role = UserRolesConstant.Admin , Password="1234"},
            new User { Id=1, Name= "John", Email= "john.dow@gmail.com",Role = UserRolesConstant.User , Password="1234"},
        };
        }

        public User Authenticate(string username, string password)
        {
            var expectedUser = users.FirstOrDefault(u => u.Name == username && u.Password == password);
            if (expectedUser != null)
            {
                return expectedUser;
            }
            return null;
        }
    }
}