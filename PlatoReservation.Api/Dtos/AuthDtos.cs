namespace PlatoReservation.Api.Dtos;

// داده‌ای که کاربر هنگام ثبت‌نام می‌فرستد
public class RegisterDto
{
    public string FullName { get; set; } = "";
    public string Email { get; set; } = "";
    public string Password { get; set; } = "";
    public string Role { get; set; } = "Artist"; // پیش‌فرض: هنرمند
}

// داده‌ای که کاربر هنگام ورود می‌فرستد
public class LoginDto
{
    public string Email { get; set; } = "";
    public string Password { get; set; } = "";
}

// پاسخی که بعد از ورود موفق به کاربر می‌دهیم
public class AuthResponseDto
{
    public string Token { get; set; } = "";
    public string FullName { get; set; } = "";
    public string Email { get; set; } = "";
    public string Role { get; set; } = "";
}
