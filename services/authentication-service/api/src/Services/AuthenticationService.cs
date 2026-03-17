using Auth.Interfaces.Services;
using Auth.Interfaces.Respositories;
using Auth.Models.ValueObjects;
using Auth.Models.Domains;

namespace Auth.Services;

public class AuthenticationService : IAuthenticationService
{
  private readonly IAuthenticationRepository _authenticationRepository;

  public AuthenticationService(IAuthenticationRepository authenticationRepository)
  {
    _authenticationRepository = authenticationRepository;
  }

  public async Task Register(UserRegistration registration)
  {
    var existingUserAccount = await _authenticationRepository.GetAccountByEmail(registration.Email);

    if (existingUserAccount != null)
    {
      throw new InvalidOperationException("Email address already in use");
    }

    var newUserAccount = new UserAccount
    {
      Email = registration.Email,
      PasswordHash = BCrypt.Net.BCrypt.HashPassword(registration.Password),
    };

    await _authenticationRepository.CreateUserAccount(newUserAccount);

    // TODO: emit event with details to create user details
  }

  public async Task<bool> Login(UserLogin login)
  {
    var Auth = await _authenticationRepository.GetAccountByEmail(login.Email);

    if (Auth == null || !BCrypt.Net.BCrypt.Verify(login.Password, Auth.PasswordHash))
    {
      return false;
    }

    return true;
  }
}
