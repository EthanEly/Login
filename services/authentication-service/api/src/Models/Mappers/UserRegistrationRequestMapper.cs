using Auth.Models.Requests;
using Auth.Models.ValueObjects;

namespace Auth.Mappers;

public static class UserRegistrationRequestMapper
{
  public static UserRegistration ToUserRegistration(this UserRegistrationRequest request)
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