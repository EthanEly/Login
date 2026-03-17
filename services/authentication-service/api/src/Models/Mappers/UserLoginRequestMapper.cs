using Auth.Models.Requests;
using Auth.Models.ValueObjects;

namespace Auth.Mappers;

public static class UserLoginMapper
{
  public static UserLogin ToUserLogin(this UserLoginRequest request)
  {
    return new UserLogin
    {
      Email = request.Email,
      Password = request.Password
    };
  }
}