using Microsoft.EntityFrameworkCore;
using PlatoReservation.Api.Entities;

namespace PlatoReservation.Api.Data;

public class AppDbContext : DbContext
{
    // سازنده کلاس: تنظیمات اتصال (مثل Connection String) را از Program.cs تحویل می‌گیرد
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    // معرفی جدول‌های دیتابیس به Entity Framework
    public DbSet<User> Users => Set<User>();
    public DbSet<Studio> Studios => Set<Studio>();
    public DbSet<StudioFeature> StudioFeatures => Set<StudioFeature>();
    public DbSet<Booking> Bookings => Set<Booking>();
    public DbSet<Payment> Payments => Set<Payment>();

    // تنظیمات اختصاصی نگاشت (Mapping) به جداول پایگاه داده
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // ۱. تعیین دقت و ارقام اعشار برای مقادیر مالی (۱۸ رقم با ۲ رقم اعشار)
        modelBuilder.Entity<Studio>()
            .Property(s => s.HourlyRate)
            .HasPrecision(18, 2);

        modelBuilder.Entity<Booking>()
            .Property(b => b.TotalPrice)
            .HasPrecision(18, 2);

        modelBuilder.Entity<Payment>()
            .Property(p => p.Amount)
            .HasPrecision(18, 2);

        // ۲. جلوگیری از حذف آبشاری (Cascade Delete):
        // اگر پلاتو یا کاربری حذف شد، رزروهای گذشته به صورت خودکار حذف نشوند تا سوابق حفظ بماند
        modelBuilder.Entity<Booking>()
            .HasOne(b => b.Studio)
            .WithMany(s => s.Bookings)
            .HasForeignKey(b => b.StudioId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Booking>()
            .HasOne(b => b.User)
            .WithMany(u => u.Bookings)
            .HasForeignKey(b => b.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
