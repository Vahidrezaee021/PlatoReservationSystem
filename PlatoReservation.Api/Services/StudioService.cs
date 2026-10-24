using Microsoft.EntityFrameworkCore;
using PlatoReservation.Api.Data;
using PlatoReservation.Api.Dtos;
using PlatoReservation.Api.Entities;

namespace PlatoReservation.Api.Services;

public class StudioService
{
    private readonly AppDbContext _db;

    public StudioService(AppDbContext db)
    {
        _db = db;
    }

    // دریافت لیست تمام پلاتوهای فعال
    public async Task<List<StudioDto>> GetAllStudiosAsync()
    {
        var studios = await _db.Studios
            .Include(s => s.Owner)
            .Include(s => s.Features)
            .Where(s => s.IsActive)
            .ToListAsync();

        return studios.Select(s => new StudioDto
        {
            Id = s.Id,
            Title = s.Title,
            Description = s.Description,
            Address = s.Address,
            City = s.City,
            AreaInMeters = s.AreaInMeters,
            HourlyRate = s.HourlyRate,
            ImageUrl = s.ImageUrl,
            IsActive = s.IsActive,
            OpenTime = s.OpenTime,
            CloseTime = s.CloseTime,
            OwnerName = s.Owner?.FullName ?? "",
            Features = s.Features.Select(f => f.Title).ToList()
        }).ToList();
    }

    // دریافت جزئیات یک پلاتو با شناسه
    public async Task<StudioDto?> GetStudioByIdAsync(int id)
    {
        var s = await _db.Studios
            .Include(x => x.Owner)
            .Include(x => x.Features)
            .FirstOrDefaultAsync(x => x.Id == id && x.IsActive);

        if (s == null) return null;

        return new StudioDto
        {
            Id = s.Id,
            Title = s.Title,
            Description = s.Description,
            Address = s.Address,
            City = s.City,
            AreaInMeters = s.AreaInMeters,
            HourlyRate = s.HourlyRate,
            ImageUrl = s.ImageUrl,
            IsActive = s.IsActive,
            OpenTime = s.OpenTime,
            CloseTime = s.CloseTime,
            OwnerName = s.Owner?.FullName ?? "",
            Features = s.Features.Select(f => f.Title).ToList()
        };
    }

    // ثبت پلاتوی جدید توسط صاحب پلاتو
    public async Task<bool> CreateStudioAsync(int ownerId, CreateStudioDto dto)
    {
        var studio = new Studio
        {
            OwnerId = ownerId,
            Title = dto.Title,
            Description = dto.Description,
            Address = dto.Address,
            City = dto.City,
            AreaInMeters = dto.AreaInMeters,
            HourlyRate = dto.HourlyRate,
            ImageUrl = dto.ImageUrl,
            IsActive = true,
            OpenTime = dto.OpenTime,
            CloseTime = dto.CloseTime
        };

        _db.Studios.Add(studio);
        await _db.SaveChangesAsync();

        if (dto.Features != null && dto.Features.Any())
        {
            foreach (var featureTitle in dto.Features)
            {
                _db.StudioFeatures.Add(new StudioFeature
                {
                    StudioId = studio.Id,
                    Title = featureTitle
                });
            }
            await _db.SaveChangesAsync();
        }

        return true;
    }
}
