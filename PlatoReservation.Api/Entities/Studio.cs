namespace PlatoReservation.Api.Entities;

public class Studio
{
    public int Id { get; set; }

    // شناسه مالک این پلاتو
    public int OwnerId { get; set; }

    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string City { get; set; } = "تهران";

    public int AreaInMeters { get; set; }
    public decimal HourlyRate { get; set; }

    public string? ImageUrl { get; set; }
    public bool IsActive { get; set; } = true;

    // ساعت کاری روزانه
    public TimeSpan OpenTime { get; set; } = new(9, 0, 0);
    public TimeSpan CloseTime { get; set; } = new(22, 0, 0);

    // مالک پلاتو
    public User Owner { get; set; } = null!;

    // امکانات و رزروهای این پلاتو
    public ICollection<StudioFeature> Features { get; set; } = new List<StudioFeature>();
    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}
