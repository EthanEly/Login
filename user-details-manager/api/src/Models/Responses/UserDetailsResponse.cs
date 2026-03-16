namespace User.Models.Responses;

public class UserDetailsResponse
{
  public required int Id { get; set; }
  public required string FirstName { get; set; }
  public required string LastName { get; set; }
  public required string Email { get; set; }
}
