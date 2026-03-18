using User.Models.Requests;
using User.Models.ValueObjects;

namespace User.Mappers;

public static class UserRegistrationMapper
{
  public static UserRegistration ToUserRegistrationEntity(this UserRegistrationRequest request)
  {
    return new UserRegistration
    {
      FirstName = request.FirstName,
      LastName = request.LastName,
      AccountId = request.AccountId,
    };
  }
}