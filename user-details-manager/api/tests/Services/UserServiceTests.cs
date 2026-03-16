using User.Interfaces.Respositories;
using User.Models.ValueObjects;
using User.Models.Domains;
using User.Services;
using Moq;

namespace User.Tests.Services;

public class UserServiceTests
{
  private readonly Mock<IUserRepository> _userRepositoryMock;
  private readonly UserService _userService;

  public UserServiceTests()
  {
    _userRepositoryMock = new Mock<IUserRepository>();
    _userService = new UserService(_userRepositoryMock.Object);
  }

  [Fact]
  public async Task Register_ShouldCreateUser_WhenEmailIsNotInUse()
  {
    // Arrange
    var registration = new UserRegistration
    {
      Email = "test@example.com",
      FirstName = "Test",
      LastName = "User",
      Password = "password123"
    };

    _userRepositoryMock.Setup(repo => repo.GetUserByEmail(registration.Email))
        .ReturnsAsync((UserEntity?)null);

    // Act
    await _userService.Register(registration);

    // Assert
    _userRepositoryMock.Verify(repo => repo.CreateUser(It.Is<UserEntity>(u =>
        u.Email == registration.Email &&
        u.FirstName == registration.FirstName &&
        u.LastName == registration.LastName &&
        BCrypt.Net.BCrypt.Verify(registration.Password, u.PasswordHash)
    )), Times.Once);
  }

  [Fact]
  public async Task Register_ShouldThrowInvalidOperationException_WhenEmailIsAlreadyInUse()
  {
    // Arrange
    var registration = new UserRegistration
    {
      Email = "test@example.com",
      Password = "password123",
      FirstName = "Test",
      LastName = "User"
    };
    var existingUser = new UserEntity
    {
      Email = registration.Email,
      FirstName = "Test",
      LastName = "User",
      PasswordHash = "GENERATED_HASH"
    };

    _userRepositoryMock.Setup(repo => repo.GetUserByEmail(registration.Email)).ReturnsAsync(existingUser);

    // Act & Assert
    var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _userService.Register(registration));
    Assert.Equal("Email address already in use", exception.Message);
    _userRepositoryMock.Verify(repo => repo.CreateUser(It.IsAny<UserEntity>()), Times.Never);
  }

  [Fact]
  public async Task Login_ShouldReturnTrue_WhenCredentialsAreValid()
  {
    // Arrange
    var login = new UserLogin { Email = "test@example.com", Password = "password123" };
    var user = new UserEntity
    {
      Email = login.Email,
      PasswordHash = BCrypt.Net.BCrypt.HashPassword(login.Password),
      FirstName = "Test",
      LastName = "User"
    };

    _userRepositoryMock.Setup(repo => repo.GetUserByEmail(login.Email)).ReturnsAsync(user);

    // Act
    var result = await _userService.Login(login);

    // Assert
    Assert.True(result);
  }

  [Fact]
  public async Task Login_ShouldReturnFalse_WhenUserDoesNotExist()
  {
    // Arrange
    var login = new UserLogin { Email = "test@example.com", Password = "password123" };

    _userRepositoryMock.Setup(repo => repo.GetUserByEmail(login.Email)).ReturnsAsync((UserEntity?)null);

    // Act
    var result = await _userService.Login(login);

    // Assert
    Assert.False(result);
  }

  [Fact]
  public async Task Login_ShouldReturnFalse_WhenPasswordIsIncorrect()
  {
    // Arrange
    var login = new UserLogin { Email = "test@example.com", Password = "password123" };
    var user = new UserEntity
    {
      Email = login.Email,
      PasswordHash = BCrypt.Net.BCrypt.HashPassword("WRONG_PASSWORD"),
      FirstName = "Test",
      LastName = "User"
    };

    _userRepositoryMock.Setup(repo => repo.GetUserByEmail(login.Email)).ReturnsAsync(user);

    // Act
    var result = await _userService.Login(login);

    // Assert
    Assert.False(result);
  }

  [Fact]
  public async Task GetUserByEmail_ShouldReturnUser_WhenUserExists()
  {
    // Arrange
    var email = "test@example.com";
    var expectedUser = new UserEntity
    {
      Email = "test@example.com",
      FirstName = "Test",
      LastName = "User",
      PasswordHash = "GENERATED_HASH"
    };

    _userRepositoryMock.Setup(repo => repo.GetUserByEmail(email)).ReturnsAsync(expectedUser);

    // Act
    var result = await _userService.GetUserByEmail(email);

    // Assert
    Assert.NotNull(result);
    Assert.Equal(expectedUser.FirstName, result.FirstName);
    Assert.Equal(expectedUser.LastName, result.LastName);
    Assert.Equal(expectedUser.PasswordHash, result.PasswordHash);
  }

  [Fact]
  public async Task GetUserByEmail_ShouldReturnNull_WhenUserDoesNotExist()
  {
    // Arrange
    var email = "test@example.com";
    _userRepositoryMock.Setup(repo => repo.GetUserByEmail(email)).ReturnsAsync((UserEntity?)null);

    // Act
    var result = await _userService.GetUserByEmail(email);

    // Assert
    Assert.Null(result);
  }
}
