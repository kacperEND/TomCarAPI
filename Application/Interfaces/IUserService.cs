using Domain.Models;

namespace Application.Interfaces
{
    public interface IUserService
    {
        User Get(int id);

        User GetByUserNameOrEmail(string userNameOrEmail);
    }
}