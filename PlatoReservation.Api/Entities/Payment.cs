namespace PlatoReservation.Api.Entities;

public class Payment
{
    public int Id { get; set; }

    public int BookingId { get; set; }
    public decimal Amount { get; set; }

    public string TrackingCode { get; set; } = string.Empty;
    public bool IsSuccessful { get; set; }

    public DateTime PaymentDate { get; set; } = DateTime.UtcNow;

    // رزروی که این پرداخت برای آن انجام شده
    public Booking Booking { get; set; } = null!;
}
