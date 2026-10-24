using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using PlatoReservation.Api.Data;
using PlatoReservation.Api.Dtos;
using PlatoReservation.Api.Entities;
using PlatoReservation.Api.Helpers;

namespace PlatoReservation.Api.Services;

public class AuthService
{
    private readonly AppDbContext _db;
    private readonly IConfiguration _config;

    public AuthService(AppDbContext db, IConfiguration config)
    {
        _db = db;
        _config = config;
    }

    // ثبت‌نام کاربر جدید
    public async Task<AuthResponseDto?> RegisterAsync(RegisterDto dto)
    {
        // بررسی اینکه ایمیل تکراری نباشد
        var exists = await _db.Users.AnyAsync(u => u.Email == dto.Email);
        if (exists) return null;

        var user = new User
        {
            FullName = dto.FullName,
            Email = dto.Email,
            PasswordHash = PasswordHasher.Hash(dto.Password),
            Role = dto.Role,
            CreatedAt = DateTime.UtcNow
        };

        _db.Users.Add(user);
        await _db.SaveChangesAsync();

        return new AuthResponseDto
        {
            Token = GenerateToken(user),
            FullName = user.FullName,
            Email = user.Email,
            Role = user.Role
        };
    }

    // ورود کاربر
    public async Task<AuthResponseDto?> LoginAsync(LoginDto dto)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
        if (user == null) return null;

        if (!PasswordHasher.Verify(dto.Password, user.PasswordHash)) return null;

        return new AuthResponseDto
        {
            Token = GenerateToken(user),
            FullName = user.FullName,
            Email = user.Email,
            Role = user.Role
        };
    }

    // ساخت توکن JWT
    private string GenerateToken(User user)
    {
        var secret = _config["Jwt:Key"];
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.FullName),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role)
        };

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(8),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
