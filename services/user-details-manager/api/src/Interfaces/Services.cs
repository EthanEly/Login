using User.Models.ValueObjects;
using User.Models.Domains;

namespace User.Interfaces.Services;

public interface IUserService
{
  Task Register(UserRegistration registration);
  Task<UserEntity?> GetUserById(int id);
}
