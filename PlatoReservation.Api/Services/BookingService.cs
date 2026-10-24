using Microsoft.EntityFrameworkCore;
using PlatoReservation.Api.Data;
using PlatoReservation.Api.Dtos;
using PlatoReservation.Api.Entities;

namespace PlatoReservation.Api.Services;

public class BookingService
{
    private readonly AppDbContext _db;

    public BookingService(AppDbContext db)
    {
        _db = db;
    }

    // ثبت رزرو جدید با بررسی دقیق تداخل زمانی
    public async Task<(bool Success, string Message, int BookingId)> CreateBookingAsync(int userId, CreateBookingDto dto)
    {
        // ۱. پیدا کردن پلاتو و بررسی فعال بودن آن
        var studio = await _db.Studios.FirstOrDefaultAsync(s => s.Id == dto.StudioId && s.IsActive);
        if (studio == null)
        {
            return (false, "پلاتوی مورد نظر یافت نشد یا غیرفعال است.", 0);
        }

        // ۲. بررسی اعتبار زمان (ساعت پایان باید بعد از ساعت شروع باشد)
        if (dto.EndTime <= dto.StartTime)
        {
            return (false, "ساعت پایان باید بعد از ساعت شروع باشد.", 0);
        }

        // ۳. بررسی تداخل زمانی (Overlap Check)
        // فرمول تداخل: رزرو جدید با رزروهای قبلی تداخل دارد اگر:
        // (StartTime < ExistingEnd) AND (EndTime > ExistingStart)
        var hasOverlap = await _db.Bookings
            .AnyAsync(b => b.StudioId == dto.StudioId &&
                           b.Status != "Cancelled" &&
                           dto.StartTime < b.EndTime &&
                           dto.EndTime > b.StartTime);

        if (hasOverlap)
        {
            return (false, "این پلاتو در بازه زمانی انتخاب شده قبلاً رزرو شده است.", 0);
        }

        // ۴. محاسبه قیمت کل
        var totalHours = (decimal)(dto.EndTime - dto.StartTime).TotalHours;
        var totalPrice = totalHours * studio.HourlyRate;

        // ۵. ایجاد رکورد رزرو
        var booking = new Booking
        {
            StudioId = dto.StudioId,
            UserId = userId,
            StartTime = dto.StartTime,
            EndTime = dto.EndTime,
            TotalPrice = totalPrice,
            Status = "PendingPayment", // در انتظار پرداخت
            CreatedAt = DateTime.UtcNow
        };

        _db.Bookings.Add(booking);
        await _db.SaveChangesAsync();

        return (true, "رزرو با موفقیت ثبت شد. لطفاً به بخش پرداخت مراجعه کنید.", booking.Id);
    }

    // گرفتن لیست رزروهای کاربر
    public async Task<List<BookingDto>> GetUserBookingsAsync(int userId)
    {
        var bookings = await _db.Bookings
            .Include(b => b.Studio)
            .Where(b => b.UserId == userId)
            .ToListAsync();

        return bookings.Select(b => new BookingDto
        {
            Id = b.Id,
            StudioId = b.StudioId,
            StudioTitle = b.Studio.Title,
            StartTime = b.StartTime,
            EndTime = b.EndTime,
            TotalPrice = b.TotalPrice,
            Status = b.Status,
            CreatedAt = b.CreatedAt
        }).ToList();
    }
}
