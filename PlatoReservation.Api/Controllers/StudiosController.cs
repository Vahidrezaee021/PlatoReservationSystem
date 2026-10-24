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

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll()
    {
        var studios = await _studioService.GetAllStudiosAsync();
        return Ok(studios);
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById(int id)
    {
        var studio = await _studioService.GetStudioByIdAsync(id);
        if (studio == null) return NotFound(new { message = "پلاتو مورد نظر یافت نشد." });
        return Ok(studio);
    }

    [HttpPost]
    [Authorize(Roles = "Owner")]
    public async Task<IActionResult> Create([FromBody] CreateStudioDto dto)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userIdClaim == null) return Unauthorized();

        int ownerId = int.Parse(userIdClaim);
        var success = await _studioService.CreateStudioAsync(ownerId, dto);
        if (!success) return BadRequest(new { message = "خطا در ثبت پلاتو." });

        return Ok(new { message = "پلاتو با موفقیت ثبت شد." });
    }
}
