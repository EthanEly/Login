using Microsoft.EntityFrameworkCore;
using Auth.Models.Domains;
using Auth.Respositories;
using Auth.Respositories.DatabaseContext;

namespace Auth.Tests.Repositories;

public class AuthenticationRepositoryTests : IDisposable
{
  private readonly AuthenticationDbContext _dbContext;
  private readonly AuthenticationRepository _authenticationRepository;

  public AuthenticationRepositoryTests()
  {
    var options = new DbContextOptionsBuilder<AuthenticationDbContext>()
        .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
        .Options;
    _dbContext = new AuthenticationDbContext(options);
    _authenticationRepository = new AuthenticationRepository(_dbContext);
  }

  [Fact]
  public async Task CreateUserAccount_ShouldCreateUserAccount()
  {
    // Arrange
    var newUser = new UserAccount
    {
      Email = "test@example.com",
      PasswordHash = "GENERATED_PASSWORD_HASH",
    };

    // Act
    await _authenticationRepository.CreateUserAccount(newUser);

    // Assert
    var createdUser = await _dbContext.UserAccounts.FirstOrDefaultAsync(u => u.Email == "test@example.com");

    Assert.NotNull(createdUser);
    Assert.Equal(newUser.Email, createdUser.Email);
    Assert.Equal(newUser.PasswordHash, createdUser.PasswordHash);
    Assert.True(createdUser.Id > 0);
  }

  [Fact]
  public async Task GetUserByEmail_ShouldReturnUserAccount_WhenUserAccountExists()
  {
    // Arrange
    var existingUser = new UserAccount
    {
      Email = "test@example.com",
      PasswordHash = "GENERATED_PASSWORD_HASH",
      Id = 123,
    };
    await _dbContext.UserAccounts.AddAsync(existingUser);
    await _dbContext.SaveChangesAsync();

    // Act
    var foundUser = await _authenticationRepository.GetAccountByEmail("test@example.com");

    // Assert
    Assert.NotNull(foundUser);
    Assert.Equal(existingUser.Email, foundUser.Email);
    Assert.Equal(existingUser.PasswordHash, foundUser.PasswordHash);
    Assert.Equal(existingUser.Id, foundUser.Id);
  }

  public void Dispose()
  {
    _dbContext.Database.EnsureDeleted();
    _dbContext.Dispose();
  }
}