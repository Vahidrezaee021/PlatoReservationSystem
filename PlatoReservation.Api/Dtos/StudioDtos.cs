namespace PlatoReservation.Api.Dtos;

// داده‌هایی که صاحب پلاتو برای ثبت پلاتوی جدید می‌فرستد
public class CreateStudioDto
{
    public string Title { get; set; } = "";
    public string Description { get; set; } = "";
    public string Address { get; set; } = "";
    public string City { get; set; } = "";
    public int AreaInMeters { get; set; }
    public decimal HourlyRate { get; set; }
    public string? ImageUrl { get; set; }
    public TimeSpan OpenTime { get; set; }
    public TimeSpan CloseTime { get; set; }
    public List<string> Features { get; set; } = new(); // امکانات پلاتو (مثلا سیستم صوتی، رقص نور و...)
}

// مدلی برای نمایش لیست پلاتوها به کاربران
public class StudioDto
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public string Description { get; set; } = "";
    public string Address { get; set; } = "";
    public string City { get; set; } = "";
    public int AreaInMeters { get; set; }
    public decimal HourlyRate { get; set; }
    public string? ImageUrl { get; set; }
    public bool IsActive { get; set; }
    public TimeSpan OpenTime { get; set; }
    public TimeSpan CloseTime { get; set; }
    public string OwnerName { get; set; } = "";
    public List<string> Features { get; set; } = new();
}
