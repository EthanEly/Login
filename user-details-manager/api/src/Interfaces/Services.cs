using User.Models.UserRegistration;
using User.Models.UserLogin;

namespace User.Interfaces.Services;

public interface IUserService
{
  Task Register(UserRegistration registration);
  Task<bool> Login(UserLogin login);
}
