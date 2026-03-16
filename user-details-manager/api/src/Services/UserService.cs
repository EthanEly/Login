using User.Interfaces.Services;
using User.Interfaces.Respositories;
using User.Models.UserRegistration;
using User.Models.UserLogin;
using User.Models.UserEntity;

namespace User.Services;

public class UserService : IUserService
{
  private readonly IUserRepository _userRepository;

  public UserService(IUserRepository userRepository)
  {
    _userRepository = userRepository;
  }

  public async Task Register(UserRegistration registration)
  {
    var existingUser = await _userRepository.GetUserByEmail(registration.Email);

    if (existingUser != null)
    {
      throw new InvalidOperationException("Email address already in use");
    }

    var newUser = new UserEntity
    {
      Email = registration.Email,
      FirstName = registration.FirstName,
      LastName = registration.LastName,
      PasswordHash = BCrypt.Net.BCrypt.HashPassword(registration.Password)
    };

    await _userRepository.CreateUser(newUser);
  }

  public async Task<bool> Login(UserLogin login)
  {
    var user = await _userRepository.GetUserByEmail(login.Email);

    if (user == null || !BCrypt.Net.BCrypt.Verify(login.Password, user.PasswordHash))
    {
      return false;
    }

    return true;
  }

  public async Task<UserEntity?> GetUserByEmail(string email)
  {
    return await _userRepository.GetUserByEmail(email);
  }
}
