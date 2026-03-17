using Auth.Models.ValueObjects;
using Auth.Models.Domains;

namespace Auth.Interfaces.Services;

public interface IAuthenticationService
{
  Task Register(UserRegistration registration);
  Task<bool> Login(UserLogin login);
}
