using User.Models.Requests;
using User.Models.ValueObjects;

namespace User.Mappers;

public static class UserLoginMapper
{
  public static UserLogin ToUserLoginEntity(this UserLoginRequest request)
  {
    return new UserLogin
    {
      Email = request.Email,
      Password = request.Password
    };
  }
}