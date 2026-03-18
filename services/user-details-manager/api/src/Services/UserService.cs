using User.Interfaces.Services;
using User.Interfaces.Respositories;
using User.Models.ValueObjects;
using User.Models.Domains;
using System.Data.Common;

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
    var existingUser = await _userRepository.GetUserById(registration.AccountId);

    if (existingUser != null)
    {
      throw new InvalidOperationException("User details already exist for ID: " + registration.AccountId.ToString() + ".");
    }

    var newUser = new UserEntity
    {
      Id = registration.AccountId,
      Email = registration.Email,
      FirstName = registration.FirstName,
      LastName = registration.LastName,
    };

    await _userRepository.CreateUser(newUser);
  }

  public async Task<int?> GetUserIdByEmail(string email)
  {
    var foundUser = await _userRepository.GetUserByEmail(email);
    if (foundUser == null)
    {
      return null;
    }
    return foundUser.Id;
  }

  public async Task<UserEntity?> GetUserById(int id)
  {
    return await _userRepository.GetUserById(id);
  }
}
