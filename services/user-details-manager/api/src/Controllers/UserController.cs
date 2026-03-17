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

    [HttpPost("/login")]
    public async Task<IActionResult> Login([FromBody] UserLoginRequest request)
    {
        var userEntity = request.ToUserLoginEntity();
        var isLoggedIn = await _userService.Login(userEntity);

        return isLoggedIn ? Ok(new { message = "Login successful" }) : Unauthorized();
    }

    [HttpGet("/details/{email}")]
    public async Task<IActionResult> GetDetails(string email)
    {
        var user = await _userService.GetUserByEmail(email);
        if (user == null)
        {
            return NotFound(new { message = "User not found" });
        }
        var mappedUser = user.ToUserDetailsResponse();
        return Ok(mappedUser);
    }
}
