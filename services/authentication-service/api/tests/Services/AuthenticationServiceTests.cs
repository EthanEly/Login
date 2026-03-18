using Auth.Interfaces.Respositories;
using Auth.Models.ValueObjects;
using Auth.Models.Domains;
using Auth.Services;
using Moq;
using Microsoft.Extensions.Configuration;

namespace Auth.Tests.Services;

public class AuthenticationServiceTests
{
  private readonly Mock<IAuthenticationRepository> _authenticationRepositoryMock;
  private readonly Mock<IConfiguration> _configurationMock;
  private readonly AuthenticationService _authenticationService;

  public AuthenticationServiceTests()
  {
    _authenticationRepositoryMock = new Mock<IAuthenticationRepository>();
    _configurationMock = new Mock<IConfiguration>();

    var jwtSettings = new Dictionary<string, string>
    {
        {"Jwt:Key", "a92c016e30094921a9719391054350117367468132924195b002135659858712"},
        {"Jwt:Issuer", "TestIssuer"},
        {"Jwt:Audience", "TestAudience"}
    };
    _configurationMock.Setup(c => c[It.IsAny<string>()]).Returns((string key) => jwtSettings.ContainsKey(key) ? jwtSettings[key] : null);

    _authenticationService = new AuthenticationService(_authenticationRepositoryMock.Object, _configurationMock.Object);
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
  public async Task Login_ShouldReturnToken_WhenCredentialsAreValid()
  {
    // Arrange
    var login = new UserLogin { Email = "test@example.com", Password = "password123" };
    var user = new UserAccount
    {
      Id = 1,
      Email = login.Email,
      PasswordHash = BCrypt.Net.BCrypt.HashPassword(login.Password)
    };

    _authenticationRepositoryMock.Setup(repo => repo.GetAccountByEmail(login.Email)).ReturnsAsync(user);

    // Act
    var result = await _authenticationService.Login(login);

    // Assert
    Assert.NotNull(result);
  }

  [Fact]
  public async Task Login_ShouldReturnNull_WhenUserDoesNotExist()
  {
    // Arrange
    var login = new UserLogin { Email = "test@example.com", Password = "password123" };

    _authenticationRepositoryMock.Setup(repo => repo.GetAccountByEmail(login.Email)).ReturnsAsync((UserAccount?)null);

    // Act
    var result = await _authenticationService.Login(login);

    // Assert
    Assert.Null(result);
  }

  [Fact]
  public async Task Login_ShouldReturnNull_WhenPasswordIsIncorrect()
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
    Assert.Null(result);
  }
}
