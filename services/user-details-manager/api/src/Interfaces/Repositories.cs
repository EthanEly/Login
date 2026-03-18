using User.Models.Domains;

namespace User.Interfaces.Respositories;

public interface IUserRepository
{
  Task CreateUser(UserEntity user);
  Task<UserEntity?> GetUserByEmail(string email);
  Task<UserEntity?> GetUserById(int id);
}
