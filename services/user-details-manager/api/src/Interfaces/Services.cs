using User.Models.ValueObjects;
using User.Models.Domains;

namespace User.Interfaces.Services;

public interface IUserService
{
  Task Register(UserRegistration registration);
  Task<int?> GetUserIdByEmail(string email);
  Task<UserEntity?> GetUserById(int id);
}
