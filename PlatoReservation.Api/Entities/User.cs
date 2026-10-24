namespace PlatoReservation.Api.Entities;

public class User
{
    public int Id { get; set; }

    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;

    // نقش کاربر: Artist یا Owner
    public string Role { get; set; } = "Artist";

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // ارتباط‌های کاربر با بخش‌های دیگر سامانه
    public ICollection<Studio> Studios { get; set; } = new List<Studio>();
    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}
