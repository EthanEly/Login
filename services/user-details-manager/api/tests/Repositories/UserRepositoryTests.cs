using System.Data.Common;
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
      Id = 456,
      FirstName = "Test",
      LastName = "User",
    };

    // Act
    await _userRepository.CreateUser(newUser);

    // Assert
    var createdUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == 456);

    Assert.NotNull(createdUser);
    Assert.Equal(newUser.Id, createdUser.Id);
    Assert.Equal(newUser.FirstName, createdUser.FirstName);
    Assert.Equal(newUser.LastName, createdUser.LastName);
  }

  [Fact]
  public async Task GetUserById_ShouldReturnUser_WhenUserExists()
  {
    // Arrange
    var existingUser = new UserEntity
    {
      Id = 456,
      FirstName = "Test",
      LastName = "User",
    };
    await _dbContext.Users.AddAsync(existingUser);
    await _dbContext.SaveChangesAsync();

    // Act
    var foundUser = await _userRepository.GetUserById(456);

    // Assert
    Assert.NotNull(foundUser);
    Assert.Equal(existingUser.FirstName, foundUser.FirstName);
    Assert.Equal(existingUser.LastName, foundUser.LastName);
  }

  public void Dispose()
  {
    _dbContext.Database.EnsureDeleted();
    _dbContext.Dispose();
  }
}