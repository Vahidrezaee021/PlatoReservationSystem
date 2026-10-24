using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using PlatoReservation.Api.Dtos;
using PlatoReservation.Api.Services;

namespace PlatoReservation.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StudiosController : ControllerBase
{
    private readonly StudioService _studioService;

    public StudiosController(StudioService studioService)
    {
        _studioService = studioService;
    }

    // GET /api/studios (همه می‌توانند ببینند - نیازی به توکن ندارد)
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var studios = await _studioService.GetAllStudiosAsync();
        return Ok(studios);
    }

    // POST /api/studios (فقط صاحبان پلاتو با داشتن توکن می‌توانند پلاتو ثبت کنند)
    [HttpPost]
    [Authorize(Roles = "Owner")]
    public async Task<IActionResult> Create([FromBody] CreateStudioDto dto)
    {
        // استخراج شناسه کاربر از توکن JWT
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userIdClaim == null) return Unauthorized();

        int ownerId = int.Parse(userIdClaim);

        var success = await _studioService.CreateStudioAsync(ownerId, dto);
        if (!success) return BadRequest(new { message = "خطا در ثبت پلاتو." });

        return Ok(new { message = "پلاتو با موفقیت ثبت شد." });
    }
}
