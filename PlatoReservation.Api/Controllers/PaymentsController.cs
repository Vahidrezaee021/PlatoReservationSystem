using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using PlatoReservation.Api.Dtos;
using PlatoReservation.Api.Services;

namespace PlatoReservation.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PaymentsController : ControllerBase
{
    private readonly PaymentService _paymentService;

    public PaymentsController(PaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    // POST /api/payments (انجام پرداخت شبیه‌سازی‌شده - نقش Artist)
    [HttpPost]
    [Authorize(Roles = "Artist")]
    public async Task<IActionResult> Pay([FromBody] CreatePaymentDto dto)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userIdClaim == null) return Unauthorized();

        int userId = int.Parse(userIdClaim);

        var result = await _paymentService.ProcessPaymentAsync(userId, dto);
        if (!result.Success)
        {
            return BadRequest(new { message = result.Message });
        }

        return Ok(new
        {
            message = result.Message,
            trackingCode = result.TrackingCode
        });
    }
}
