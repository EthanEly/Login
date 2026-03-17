using Auth.Models.Requests;
using Auth.Mappers;

namespace Auth.Tests.Mappers;

public class UserLoginRequestMapperTests
{
  [Fact]
  public void ToUserLogin_ShouldMapCorrectly()
  {
    // Arrange
    var request = new UserLoginRequest
    {
      Email = "test@example.com",
      Password = "password123"
    };

    // Act
    var entity = request.ToUserLogin();

    // Assert
    Assert.Equal(entity.Email, request.Email);
    Assert.Equal(entity.Password, request.Password);
  }
}