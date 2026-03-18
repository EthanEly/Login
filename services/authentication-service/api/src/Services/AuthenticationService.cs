using Auth.Interfaces.Services;
using Auth.Interfaces.Respositories;
using Auth.Models.ValueObjects;
using Auth.Models.Domains;
using Auth.Clients;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Auth.Services;

public class AuthenticationService : IAuthenticationService
{
  private readonly IAuthenticationRepository _authenticationRepository;
  private readonly UserDetailsClient _userDetailsClient;
  private readonly IConfiguration _configuration;

  public AuthenticationService(
    IAuthenticationRepository authenticationRepository,
    IConfiguration configuration,
    UserDetailsClient userDetailsClient)
  {
    _authenticationRepository = authenticationRepository;
    _configuration = configuration;
    _userDetailsClient = userDetailsClient;
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

    var createdUser = await _authenticationRepository.GetAccountByEmail(newUserAccount.Email);

    // A more robust solution is to emit an event with details 
    // This allows other services to subscribe and react accordingly to a user account being registered
    // For the time being, we'll settle for an HTTP request
    await _userDetailsClient.RegisterUser(createdUser.Id, createdUser.Email, registration.FirstName, registration.LastName);
  }

  public async Task<string?> Login(UserLogin login)
  {
    var userAccount = await _authenticationRepository.GetAccountByEmail(login.Email);

    if (userAccount == null || !BCrypt.Net.BCrypt.Verify(login.Password, userAccount.PasswordHash))
    {
      return null;
    }

    var tokenHandler = new JwtSecurityTokenHandler();
    var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);

    var claims = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, userAccount.Id.ToString()),
        new Claim(ClaimTypes.Email, userAccount.Email),
    };

    var tokenDescriptor = new SecurityTokenDescriptor
    {
      Subject = new ClaimsIdentity(claims),
      Expires = DateTime.UtcNow.AddHours(1),
      Issuer = _configuration["Jwt:Issuer"],
      Audience = _configuration["Jwt:Audience"],
      SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
    };

    var token = tokenHandler.CreateToken(tokenDescriptor);
    return tokenHandler.WriteToken(token);
  }
}
