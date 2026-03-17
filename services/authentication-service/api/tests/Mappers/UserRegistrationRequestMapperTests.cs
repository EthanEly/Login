using Auth.Models.Requests;
using Auth.Mappers;

namespace Auth.Tests.Mappers;

public class UserRegistrationRequestMapperTests
{
  [Fact]
  public void ToUserRegistration_ShouldMapCorrectly()
  {
    // Arrange
    var request = new UserRegistrationRequest
    {
      Email = "test@example.com",
      FirstName = "Test",
      LastName = "User",
      Password = "password123"
    };

    // Act
    var entity = request.ToUserRegistration();

    // Assert
    Assert.Equal(entity.FirstName, request.FirstName);
    Assert.Equal(entity.LastName, request.LastName);
    Assert.Equal(entity.Email, request.Email);
    Assert.Equal(entity.Password, request.Password);
  }
}