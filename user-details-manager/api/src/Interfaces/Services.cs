using User.Models.UserRegistration;
using User.Models.UserLogin;
using User.Models.UserEntity;

namespace User.Interfaces.Services;

public interface IUserService
{
  Task Register(UserRegistration registration);
  Task<bool> Login(UserLogin login);
  Task<UserEntity?> GetUserByEmail(string email);
}
