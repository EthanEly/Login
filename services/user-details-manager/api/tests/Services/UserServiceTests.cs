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
  public async Task Register_ShouldCreateUser_WhenUserDetailsDoNotExist()
  {
    // Arrange
    var id = 456;
    var registration = new UserRegistration
    {
      AccountId = 456,
      Email = "test@example.com",
      FirstName = "Test",
      LastName = "User",
    };

    _userRepositoryMock.Setup(repo => repo.GetUserById(id))
        .ReturnsAsync((UserEntity?)null);

    // Act
    await _userService.Register(registration);

    // Assert
    _userRepositoryMock.Verify(repo => repo.CreateUser(It.Is<UserEntity>(u =>
        u.Id == registration.AccountId &&
        u.Email == registration.Email &&
        u.FirstName == registration.FirstName &&
        u.LastName == registration.LastName
    )), Times.Once);
  }

  [Fact]
  public async Task Register_ShouldThrowInvalidOperationException_WhenUserDetailsAlreadyExists()
  {
    // Arrange
    var registration = new UserRegistration
    {
      AccountId = 456,
      Email = "test@example.com",
      FirstName = "Test",
      LastName = "User"
    };
    var existingUser = new UserEntity
    {
      Id = 456,
      Email = "test@example.com",
      FirstName = "Test",
      LastName = "User",
    };

    _userRepositoryMock.Setup(repo => repo.GetUserById(registration.AccountId)).ReturnsAsync(existingUser);

    // Act & Assert
    var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _userService.Register(registration));
    Assert.Equal("User details already exist for ID: " + registration.AccountId.ToString() + ".", exception.Message);
    _userRepositoryMock.Verify(repo => repo.CreateUser(It.IsAny<UserEntity>()), Times.Never);
  }


  [Fact]
  public async Task GetUserIdByEmail_ShouldReturnUser_WhenUserExists()
  {
    // Arrange
    var email = "test@example.com";
    var expectedUser = new UserEntity
    {
      Id = 456,
      Email = "test@example.com",
      FirstName = "Test",
      LastName = "User",
    };

    _userRepositoryMock.Setup(repo => repo.GetUserByEmail(email)).ReturnsAsync(expectedUser);

    // Act
    var result = await _userService.GetUserIdByEmail(email);

    // Assert
    Assert.NotNull(result);
    Assert.Equal(expectedUser.Id, result);
  }

  [Fact]
  public async Task GetUserIdByEmail_ShouldReturnNull_WhenUserDoesNotExist()
  {
    // Arrange
    var email = "test@example.com";
    _userRepositoryMock.Setup(repo => repo.GetUserByEmail(email)).ReturnsAsync((UserEntity?)null);

    // Act
    var result = await _userService.GetUserIdByEmail(email);

    // Assert
    Assert.Null(result);
  }

  [Fact]
  public async Task GetUserById_ShouldReturnUser_WhenUserExists()
  {
    // Arrange
    var id = 456;
    var expectedUser = new UserEntity
    {
      Id = 456,
      Email = "test@example.com",
      FirstName = "Test",
      LastName = "User",
    };

    _userRepositoryMock.Setup(repo => repo.GetUserById(id)).ReturnsAsync(expectedUser);

    // Act
    var result = await _userService.GetUserById(id);

    // Assert
    Assert.NotNull(result);
    Assert.Equal(id, result.Id);
    Assert.Equal(expectedUser.FirstName, result.FirstName);
    Assert.Equal(expectedUser.LastName, result.LastName);
  }

  [Fact]
  public async Task GetUserById_ShouldReturnNull_WhenUserDoesNotExist()
  {
    // Arrange
    var id = 456;
    _userRepositoryMock.Setup(repo => repo.GetUserById(id)).ReturnsAsync((UserEntity?)null);

    // Act
    var result = await _userService.GetUserById(id);

    // Assert
    Assert.Null(result);
  }
}
