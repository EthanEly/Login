using User.Models.UserEntity;
using User.Models.UserDetailsResponse;

namespace User.Mappers;

public static class UserDetailsResponseMapper
{
  public static UserDetailsResponse ToUserDetailsResponse(this UserEntity entity)
  {
    return new UserDetailsResponse
    {
      Id = entity.Id,
      FirstName = entity.FirstName,
      LastName = entity.LastName,
      Email = entity.Email,
    };
  }
}