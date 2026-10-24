namespace PlatoReservation.Api.Dtos;

// اطلاعاتی که هنرمند برای رزرو پلاتو می‌فرستد
public class CreateBookingDto
{
    public int StudioId { get; set; }
    public DateTime StartTime { get; set; } // مثال: 2026-10-25T14:00:00
    public DateTime EndTime { get; set; }   // مثال: 2026-10-25T16:00:00
}

// مدلی برای نمایش اطلاعات رزرو به کاربر
public class BookingDto
{
    public int Id { get; set; }
    public int StudioId { get; set; }
    public string StudioTitle { get; set; } = "";
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public decimal TotalPrice { get; set; }
    public string Status { get; set; } = "";
    public DateTime CreatedAt { get; set; }
}
