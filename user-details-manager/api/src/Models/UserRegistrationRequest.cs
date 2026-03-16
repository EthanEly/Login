using System.Text.Json.Serialization;

namespace User.Models.Requests;

public class UserRegistrationRequest
{
  [JsonPropertyName("firstName")]
  public string FirstName { get; set; } = string.Empty;

  [JsonPropertyName("lastName")]
  public string LastName { get; set; } = string.Empty;

  [JsonPropertyName("email")]
  public string Email { get; set; } = string.Empty;

  [JsonPropertyName("password")]
  public string Password { get; set; } = string.Empty;
}