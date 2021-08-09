using CMS.DataAccess;
using System.Threading.Tasks;

namespace CMS.Services.Interfaces
{
    public interface IAuthService
    {
        bool CreateUser(User user, string Password);
        Task<bool> SignOut();
        User AuthenticateUser(string Username, string Password);
        User GetUser(string Username);
        User FindByMail(string Email);
        User FindById(string UserId);
        bool UpdateUser(User user);
    }

}
