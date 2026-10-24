using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using PlatoReservation.Api.Dtos;
using PlatoReservation.Api.Services;

namespace PlatoReservation.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize] // فقط کاربران لاگین‌کرده می‌توانند رزرو کنند
public class BookingsController : ControllerBase
{
    private readonly BookingService _bookingService;

    public BookingsController(BookingService bookingService)
    {
        _bookingService = bookingService;
    }

    // POST /api/bookings (ثبت رزرو جدید - نقش Artist)
    [HttpPost]
    [Authorize(Roles = "Artist")]
    public async Task<IActionResult> Create([FromBody] CreateBookingDto dto)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userIdClaim == null) return Unauthorized();

        int userId = int.Parse(userIdClaim);

        var result = await _bookingService.CreateBookingAsync(userId, dto);
        if (!result.Success)
        {
            return BadRequest(new { message = result.Message });
        }

        return Ok(new { message = result.Message, bookingId = result.BookingId });
    }

    // GET /api/bookings (مشاهده رزروهای کاربر جاری)
    [HttpGet]
    public async Task<IActionResult> GetMyBookings()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userIdClaim == null) return Unauthorized();

        int userId = int.Parse(userIdClaim);
        var bookings = await _bookingService.GetUserBookingsAsync(userId);

        return Ok(bookings);
    }
}
