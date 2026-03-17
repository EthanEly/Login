using Microsoft.AspNetCore.Mvc;
using Auth.Interfaces.Services;
using Auth.Mappers;
using Auth.Models.Requests;

namespace Auth.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthenticationController : ControllerBase
{
    private readonly IAuthenticationService _authenticationService;

    public AuthenticationController(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    [HttpPost("/register")]
    public async Task<IActionResult> Register([FromBody] UserRegistrationRequest request)
    {
        try
        {
            var userAccount = request.ToUserRegistration();
            await _authenticationService.Register(userAccount);

            return Ok(new { message = "User Account registered successfully" });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    [HttpPost("/login")]
    public async Task<IActionResult> Login([FromBody] UserLoginRequest request)
    {
        var userAccount = request.ToUserLogin();
        var isLoggedIn = await _authenticationService.Login(userAccount);

        return isLoggedIn ? Ok(new { message = "Login successful" }) : Unauthorized();
    }
}
