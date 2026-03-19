using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using User.Interfaces.Services;
using User.Mappers;
using User.Models.Requests;

namespace User.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("/register")]
    public async Task<IActionResult> Register([FromBody] UserRegistrationRequest request)
    {
        try
        {
            var userEntity = request.ToUserRegistrationEntity();
            await _userService.Register(userEntity);

            return Ok(new { message = "User registered successfully" });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    [HttpGet("/details/{id}")]
    [Authorize]
    public async Task<IActionResult> GetDetails(int id)
    {
        var user = await _userService.GetUserById(id);
        if (user == null)
        {
            return NotFound(new { message = "User not found" });
        }
        var mappedUser = user.ToUserDetailsResponse();
        return Ok(mappedUser);
    }
}
