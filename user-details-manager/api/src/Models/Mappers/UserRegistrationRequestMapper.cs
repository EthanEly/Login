using User.Models.Requests;
using User.Models.UserRegistration;

namespace User.Mappers;

public static class UserRegistrationMapper
{
  public static UserRegistration ToUserRegistrationEntity(this UserRegistrationRequest request)
  {
    return new UserRegistration
    {
      FirstName = request.FirstName,
      LastName = request.LastName,
      Email = request.Email,
      Password = request.Password
    };
  }
}