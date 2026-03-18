using User.Models.Responses;
using User.Models.Domains;

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
    };
  }
}