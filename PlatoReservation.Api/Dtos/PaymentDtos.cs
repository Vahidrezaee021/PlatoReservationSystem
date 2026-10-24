namespace PlatoReservation.Api.Dtos;

// داده‌هایی که برای پرداخت ارسال می‌شود
public class CreatePaymentDto
{
    public int BookingId { get; set; }
}

// نمایش اطلاعات پرداخت به کاربر
public class PaymentDto
{
    public int Id { get; set; }
    public int BookingId { get; set; }
    public decimal Amount { get; set; }
    public string TrackingCode { get; set; } = "";
    public bool IsSuccessful { get; set; }
    public DateTime PaymentDate { get; set; }
}
