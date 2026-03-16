using Xunit;
using User.Mappers;
using User.Models.Requests;

namespace User.Tests.Mappers;

public class UserRegistrationRequestMapperTests
{
  [Fact]
  public void ToUserRegistrationEntity_ShouldMapCorrectly()
  {
    // Arrange
    var request = new UserRegistrationRequest
    {
      FirstName = "Test",
      LastName = "User",
      Email = "test@example.com",
      Password = "password123"
    };

    // Act
    var entity = request.ToUserRegistrationEntity();

    // Assert
    Assert.Equal(entity.FirstName, request.FirstName);
    Assert.Equal(entity.LastName, request.LastName);
    Assert.Equal(entity.Email, request.Email);
    Assert.Equal(entity.Password, request.Password);
  }
}