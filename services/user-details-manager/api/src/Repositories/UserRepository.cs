using Microsoft.EntityFrameworkCore;
using User.Respositories.DatabaseContext;
using User.Interfaces.Respositories;
using User.Models.Domains;

namespace User.Respositories;

public class UserRepository : IUserRepository
{
  private readonly UserDbContext _dbContext;

  public UserRepository(UserDbContext dbContext)
  {
    _dbContext = dbContext;
  }

  public async Task CreateUser(UserEntity user)
  {
    await _dbContext.Users.AddAsync(user);
    await _dbContext.SaveChangesAsync();
  }

  public async Task<UserEntity?> GetUserByEmail(string email)
  {
    return await _dbContext.Users.FirstOrDefaultAsync(user => user.Email == email);
  }

  public async Task<UserEntity?> GetUserById(int id)
  {
    return await _dbContext.Users.FirstOrDefaultAsync(user => user.Id == id);
  }
}