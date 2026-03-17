using Auth.Models.Domains;

namespace Auth.Interfaces.Respositories;

public interface IAuthenticationRepository
{
  Task CreateUserAccount(UserAccount userAccount);
  Task<UserAccount?> GetAccountByEmail(string email);
}
