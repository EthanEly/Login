using User.Models.ValueObjects;
using User.Models.Domains;

namespace User.Interfaces.Services;

public interface IUserService
{
  Task Register(UserRegistration registration);
  Task<bool> Login(UserLogin login);
  Task<UserEntity?> GetUserByEmail(string email);
}
