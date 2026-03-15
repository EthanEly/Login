using Microsoft.EntityFrameworkCore;
using User.Interfaces.Respositories;
using User.Models.UserEntity;

namespace User.Respositories.UserRepository;

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
}

public class UserDbContext : DbContext
{
  public UserDbContext(DbContextOptions<UserDbContext> options) : base(options)
  {
  }

  public DbSet<UserEntity> Users { get; set; }
}