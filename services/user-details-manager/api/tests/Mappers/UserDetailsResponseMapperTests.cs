using User.Models.Domains;
using User.Mappers;

namespace User.Tests.Mappers;

public class UserDetailsResponseMapperTests
{
  [Fact]
  public void ToUserDetailsResponse_ShouldMapCorrectly()
  {
    // Arrange
    var entity = new UserEntity
    {
      Id = 1,
      FirstName = "Test",
      LastName = "User",
      Email = "test@example.com",
    };

    // Act
    var response = entity.ToUserDetailsResponse();

    // Assert
    Assert.Equal(response.Id, entity.Id);
    Assert.Equal(response.FirstName, entity.FirstName);
    Assert.Equal(response.LastName, entity.LastName);
    Assert.Equal(response.Email, entity.Email);
  }
}