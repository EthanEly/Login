using User.Models.Domains;

namespace User.Interfaces.Respositories;

public interface IUserRepository
{
  Task CreateUser(UserEntity user);
  Task<UserEntity?> GetUserById(int id);
}
