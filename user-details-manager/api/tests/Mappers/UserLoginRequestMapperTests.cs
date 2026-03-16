using User.Models.Requests;
using User.Mappers;

namespace User.Tests.Mappers;

public class UserLoginRequestMapperTests
{
  [Fact]
  public void ToUserLoginEntity_ShouldMapCorrectly()
  {
    // Arrange
    var request = new UserLoginRequest
    {
      Email = "test@example.com",
      Password = "password123"
    };

    // Act
    var entity = request.ToUserLoginEntity();

    // Assert
    Assert.Equal(entity.Email, request.Email);
    Assert.Equal(entity.Password, request.Password);
  }
}