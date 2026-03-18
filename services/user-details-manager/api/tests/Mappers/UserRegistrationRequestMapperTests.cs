using User.Models.Requests;
using User.Mappers;

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
      AccountId = 456
    };

    // Act
    var entity = request.ToUserRegistrationEntity();

    // Assert
    Assert.Equal(entity.FirstName, request.FirstName);
    Assert.Equal(entity.LastName, request.LastName);
    Assert.Equal(entity.AccountId, request.AccountId);
  }
}