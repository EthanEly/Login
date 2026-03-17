using Microsoft.EntityFrameworkCore;
using Auth.Respositories.DatabaseContext;
using Auth.Interfaces.Respositories;
using Auth.Models.Domains;

namespace Auth.Respositories;

public class AuthenticationRepository : IAuthenticationRepository
{
  private readonly AuthenticationDbContext _dbContext;

  public AuthenticationRepository(AuthenticationDbContext dbContext)
  {
    _dbContext = dbContext;
  }

  public async Task CreateUserAccount(UserAccount userAccount)
  {
    await _dbContext.UserAccounts.AddAsync(userAccount);
    await _dbContext.SaveChangesAsync();
  }

  public async Task<UserAccount?> GetAccountByEmail(string email)
  {
    return await _dbContext.UserAccounts.FirstOrDefaultAsync(userAccount => userAccount.Email == email);
  }
}