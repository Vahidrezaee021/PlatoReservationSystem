using Microsoft.EntityFrameworkCore;
using PlatoReservation.Api.Data;
using PlatoReservation.Api.Dtos;
using PlatoReservation.Api.Entities;

namespace PlatoReservation.Api.Services;

public class PaymentService
{
    private readonly AppDbContext _db;

    public PaymentService(AppDbContext db)
    {
        _db = db;
    }

    // شبیه‌سازی پرداخت برای یک رزرو
    public async Task<(bool Success, string Message, string TrackingCode)> ProcessPaymentAsync(int userId, CreatePaymentDto dto)
    {
        // ۱. پیدا کردن رزرو و بررسی اینکه متعلق به همین کاربر باشد
        var booking = await _db.Bookings
            .Include(b => b.Studio)
            .FirstOrDefaultAsync(b => b.Id == dto.BookingId && b.UserId == userId);

        if (booking == null)
        {
            return (false, "رزرو مورد نظر یافت نشد.", "");
        }

        if (booking.Status == "Confirmed")
        {
            return (false, "این رزرو قبلاً پرداخت و تایید شده است.", "");
        }

        // ۲. ساخت کد پیگیری تصادفی (شبیه‌سازی درگاه بانکی)
        var trackingCode = new Random().Next(100000, 999999).ToString();

        // ۳. ثبت اطلاعات پرداخت
        var payment = new Payment
        {
            BookingId = booking.Id,
            Amount = booking.TotalPrice,
            TrackingCode = trackingCode,
            IsSuccessful = true,
            PaymentDate = DateTime.UtcNow
        };

        _db.Payments.Add(payment);

        // ۴. آپدیت وضعیت رزرو به تاییدشده
        booking.Status = "Confirmed";

        await _db.SaveChangesAsync();

        return (true, "پرداخت با موفقیت انجام شد.", trackingCode);
    }
}
