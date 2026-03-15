using User.Models.UserEntity;

namespace User.Interfaces.Respositories;

public interface IUserRepository
{
  Task CreateUser(UserEntity user);
  Task<UserEntity?> GetUserByEmail(string email);
}
