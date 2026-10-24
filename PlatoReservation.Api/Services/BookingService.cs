using Microsoft.EntityFrameworkCore;
using PlatoReservation.Api.Data;
using PlatoReservation.Api.Dtos;
using PlatoReservation.Api.Entities;

namespace PlatoReservation.Api.Services
{
    public interface IBookingService
    {
        Task<(bool Success, string Message, BookingDto? Data)> CreateBookingAsync(int userId, CreateBookingDto dto);
        Task<List<BookingDto>> GetStudioBookingsAsync(int studioId);
        Task<bool> DeleteBookingAsync(int bookingId, int userId);
    }

    public class BookingService : IBookingService
    {
        private readonly AppDbContext _context;

        public BookingService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<(bool Success, string Message, BookingDto? Data)> CreateBookingAsync(int userId, CreateBookingDto dto)
        {
            var studio = await _context.Studios.FindAsync(dto.StudioId);
            if (studio == null || !studio.IsActive)
                return (false, "پلاتوی مورد نظر یافت نشد یا غیرفعال است.", null);

            if (dto.EndTime <= dto.StartTime)
                return (false, "زمان پایان باید بعد از زمان شروع باشد.", null);

            // بررسی تداخل زمان رزرو
            var hasOverlap = await _context.Bookings.AnyAsync(b =>
                b.StudioId == dto.StudioId &&
                b.Status != "Cancelled" &&
                dto.StartTime < b.EndTime &&
                b.StartTime < dto.EndTime
            );

            if (hasOverlap)
                return (false, "این بازه زمانی قبلاً توسط شخص دیگری رزرو شده است.", null);

            // محاسبه مبلغ کل بر اساس نرخ ساعتی پلاتو
            var hours = (decimal)(dto.EndTime - dto.StartTime).TotalHours;
            if (hours <= 0) return (false, "بازه زمانی نامعتبر است.", null);

            decimal totalPrice = Math.Round(hours * studio.HourlyRate, 0);

            var booking = new Booking
            {
                StudioId = dto.StudioId,
                UserId = userId,
                StartTime = dto.StartTime,
                EndTime = dto.EndTime,
                TotalPrice = totalPrice,
                Status = "Pending",
                CreatedAt = DateTime.UtcNow
            };

            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();

            var resultDto = new BookingDto
            {
                Id = booking.Id,
                StudioId = booking.StudioId,
                StudioTitle = studio.Title, // <-- اصلاح شد به Title
                StartTime = booking.StartTime,
                EndTime = booking.EndTime,
                TotalPrice = booking.TotalPrice,
                Status = booking.Status,
                CreatedAt = booking.CreatedAt
            };

            return (true, "رزرو با موفقیت ثبت شد.", resultDto);
        }

        public async Task<List<BookingDto>> GetStudioBookingsAsync(int studioId)
        {
            return await _context.Bookings
                .Where(b => b.StudioId == studioId && b.Status != "Cancelled")
                .Include(b => b.Studio)
                .Select(b => new BookingDto
                {
                    Id = b.Id,
                    StudioId = b.StudioId,
                    StudioTitle = b.Studio != null ? b.Studio.Title : "پلاتو", // <-- اصلاح شد به Title
                    StartTime = b.StartTime,
                    EndTime = b.EndTime,
                    TotalPrice = b.TotalPrice,
                    Status = b.Status,
                    CreatedAt = b.CreatedAt
                })
                .ToListAsync();
        }

        public async Task<bool> DeleteBookingAsync(int bookingId, int userId)
        {
            var booking = await _context.Bookings.FindAsync(bookingId);
            if (booking == null || booking.UserId != userId) return false;

            _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
