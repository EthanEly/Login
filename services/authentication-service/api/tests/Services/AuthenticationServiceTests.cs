using Auth.Interfaces.Respositories;
using Auth.Models.ValueObjects;
using Auth.Models.Domains;
using Auth.Services;
using Moq;
using System.Data.Common;

namespace Auth.Tests.Services;

public class AuthenticationServiceTests
{
  private readonly Mock<IAuthenticationRepository> _authenticationRepositoryMock;
  private readonly AuthenticationService _authenticationService;

  public AuthenticationServiceTests()
  {
    _authenticationRepositoryMock = new Mock<IAuthenticationRepository>();
    _authenticationService = new AuthenticationService(_authenticationRepositoryMock.Object);
  }

  [Fact]
  public async Task Register_ShouldCreateUserAccountAccount_WhenEmailIsNotInUse()
  {
    // Arrange
    var registration = new UserRegistration
    {
      Email = "test@example.com",
      FirstName = "Test",
      LastName = "User",
      Password = "password123"
    };

    _authenticationRepositoryMock.Setup(repo => repo.GetAccountByEmail(registration.Email))
        .ReturnsAsync((UserAccount?)null);

    // Act
    await _authenticationService.Register(registration);

    // Assert
    _authenticationRepositoryMock.Verify(repo => repo.CreateUserAccount(It.Is<UserAccount>(u =>
        u.Email == registration.Email &&
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
      FirstName = "Test",
      LastName = "User",
      Password = "password123"
    };
    var existingUser = new UserAccount
    {
      Id = 456,
      Email = registration.Email,
      PasswordHash = "GENERATED_HASH",
    };

    _authenticationRepositoryMock.Setup(repo => repo.GetAccountByEmail(registration.Email)).ReturnsAsync(existingUser);

    // Act & Assert
    var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _authenticationService.Register(registration));
    Assert.Equal("Email address already in use", exception.Message);
    _authenticationRepositoryMock.Verify(repo => repo.CreateUserAccount(It.IsAny<UserAccount>()), Times.Never);
  }

  [Fact]
  public async Task Login_ShouldReturnTrue_WhenCredentialsAreValid()
  {
    // Arrange
    var login = new UserLogin { Email = "test@example.com", Password = "password123" };
    var user = new UserAccount
    {
      Email = login.Email,
      PasswordHash = BCrypt.Net.BCrypt.HashPassword(login.Password)
    };

    _authenticationRepositoryMock.Setup(repo => repo.GetAccountByEmail(login.Email)).ReturnsAsync(user);

    // Act
    var result = await _authenticationService.Login(login);

    // Assert
    Assert.True(result);
  }

  [Fact]
  public async Task Login_ShouldReturnFalse_WhenUserDoesNotExist()
  {
    // Arrange
    var login = new UserLogin { Email = "test@example.com", Password = "password123" };

    _authenticationRepositoryMock.Setup(repo => repo.GetAccountByEmail(login.Email)).ReturnsAsync((UserAccount?)null);

    // Act
    var result = await _authenticationService.Login(login);

    // Assert
    Assert.False(result);
  }

  [Fact]
  public async Task Login_ShouldReturnFalse_WhenPasswordIsIncorrect()
  {
    // Arrange
    var login = new UserLogin { Email = "test@example.com", Password = "password123" };
    var user = new UserAccount
    {
      Email = login.Email,
      PasswordHash = BCrypt.Net.BCrypt.HashPassword("WRONG_PASSWORD")
    };

    _authenticationRepositoryMock.Setup(repo => repo.GetAccountByEmail(login.Email)).ReturnsAsync(user);

    // Act
    var result = await _authenticationService.Login(login);

    // Assert
    Assert.False(result);
  }
}
