using Microsoft.EntityFrameworkCore;
using User.Models.Domains;
using User.Respositories;
using User.Respositories.DatabaseContext;

namespace User.Tests.Repositories;

public class UserRepositoryTests : IDisposable
{
  private readonly UserDbContext _dbContext;
  private readonly UserRepository _userRepository;

  public UserRepositoryTests()
  {
    var options = new DbContextOptionsBuilder<UserDbContext>()
        .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
        .Options;
    _dbContext = new UserDbContext(options);
    _userRepository = new UserRepository(_dbContext);
  }

  [Fact]
  public async Task CreateUser_ShouldCreateUser()
  {
    // Arrange
    var newUser = new UserEntity
    {
      Email = "test@example.com",
      FirstName = "Test",
      LastName = "User",
      PasswordHash = "GENERATED_PASSWORD_HASH"
    };

    // Act
    await _userRepository.CreateUser(newUser);

    // Assert
    var createdUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == "test@example.com");

    Assert.NotNull(createdUser);
    Assert.Equal(newUser.Email, createdUser.Email);
    Assert.Equal(newUser.FirstName, createdUser.FirstName);
    Assert.Equal(newUser.LastName, createdUser.LastName);
    Assert.Equal(newUser.PasswordHash, createdUser.PasswordHash);
  }

  [Fact]
  public async Task GetUserByEmail_ShouldReturnUser_WhenUserExists()
  {
    // Arrange
    var existingUser = new UserEntity
    {
      Email = "test@example.com",
      FirstName = "Test",
      LastName = "User",
      PasswordHash = "GENERATED_PASSWORD_HASH"
    };
    await _dbContext.Users.AddAsync(existingUser);
    await _dbContext.SaveChangesAsync();

    // Act
    var foundUser = await _userRepository.GetUserByEmail("test@example.com");

    // Assert
    Assert.NotNull(foundUser);
    Assert.Equal(existingUser.Email, foundUser.Email);
    Assert.Equal(existingUser.FirstName, foundUser.FirstName);
    Assert.Equal(existingUser.LastName, foundUser.LastName);
  }

  public void Dispose()
  {
    _dbContext.Database.EnsureDeleted();
    _dbContext.Dispose();
  }
}