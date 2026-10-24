using Microsoft.AspNetCore.Mvc;
using PlatoReservation.Api.Dtos;
using PlatoReservation.Api.Services;

namespace PlatoReservation.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;

    public AuthController(AuthService authService)
    {
        _authService = authService;
    }

    // POST /api/auth/register
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto dto)
    {
        var result = await _authService.RegisterAsync(dto);

        if (result == null)
        {
            return BadRequest(new { message = "این ایمیل قبلاً ثبت شده است." });
        }

        return Ok(result);
    }

    // POST /api/auth/login
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        var result = await _authService.LoginAsync(dto);

        if (result == null)
        {
            return Unauthorized(new { message = "ایمیل یا رمز عبور اشتباه است." });
        }

        return Ok(result);
    }
}
