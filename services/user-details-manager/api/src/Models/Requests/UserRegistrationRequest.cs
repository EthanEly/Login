using System.Text.Json.Serialization;

namespace User.Models.Requests;

public class UserRegistrationRequest
{
  [JsonPropertyName("firstName")]
  public string FirstName { get; set; } = string.Empty;

  [JsonPropertyName("lastName")]
  public string LastName { get; set; } = string.Empty;

  [JsonPropertyName("accountId")]
  public int AccountId { get; set; } = 0;
}