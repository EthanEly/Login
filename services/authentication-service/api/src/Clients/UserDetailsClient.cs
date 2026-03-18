using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Auth.Clients;

public class UserDetailsClient
{
  private readonly HttpClient _httpClient;

  public UserDetailsClient(HttpClient httpClient)
  {
    _httpClient = httpClient;
  }

  public async Task RegisterUser(int id, string email, string firstName, string lastName)
  {
    var response = await _httpClient.PostAsJsonAsync("/register", new { AccountId = id, Email = email, FirstName = firstName, LastName = lastName });
    response.EnsureSuccessStatusCode();
  }
}