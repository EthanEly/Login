using Auth.Models.ValueObjects;
using Auth.Models.Domains;

namespace Auth.Interfaces.Services;

public interface IAuthenticationService
{
  Task Register(UserRegistration registration);
  Task<string?> Login(UserLogin login);
}
