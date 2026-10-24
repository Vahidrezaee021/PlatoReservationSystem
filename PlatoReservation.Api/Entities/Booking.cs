namespace PlatoReservation.Api.Entities;

public class Booking
{
    public int Id { get; set; }

    public int StudioId { get; set; }
    public int UserId { get; set; }

    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }

    public decimal TotalPrice { get; set; }

    // وضعیت رزرو: Pending، Confirmed یا Canceled
    public string Status { get; set; } = "Pending";

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // ارتباط با پلاتو و کاربری که رزرو کرده
    public Studio Studio { get; set; } = null!;
    public User User { get; set; } = null!;

    // پرداخت ممکن است هنوز ساخته نشده باشد
    public Payment? Payment { get; set; }
}
