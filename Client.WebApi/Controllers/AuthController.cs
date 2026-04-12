using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Application.Services.Auth;
using Application.Services.Auth.Request.Login;
using Application.Services.Auth.Request.Register;

namespace Client.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var user = await _authService.RegisterAsync(request);
        return Ok(new { message = "Registered successfully", userId = user.Id });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var user = await _authService.LoginAsync(request);

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim("SessionId", Guid.NewGuid().ToString())
        };

        foreach (var role in user.Roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role.Name));
        }

        var identity = new ClaimsIdentity(claims, "CookieAuth");
        var principal = new ClaimsPrincipal(identity);

        await HttpContext.SignInAsync("CookieAuth", principal);

        return Ok(new { message = "Logged in successfully" });
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync("CookieAuth");
        return Ok(new { message = "Logged out" });
    }
}