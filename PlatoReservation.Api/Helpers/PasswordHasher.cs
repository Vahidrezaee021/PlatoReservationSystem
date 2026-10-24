using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace PlatoReservation.Api.Helpers;

public static class PasswordHasher
{
    // برای نمک (Salt) هش
    public static string Hash(string password)
    {
        // یک نمک تصادفی ۱۶ بایتی بساز
        byte[] salt = RandomNumberGenerator.GetBytes(16);

        // هش استاندارد PBKDF2 بساز
        string hash = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 100000,
            numBytesRequested: 32));

        // نمک را به ابتدای هش اضافه می‌کنیم تا بعداً برای مقایسه در دسترس باشد
        return $"{Convert.ToBase64String(salt)}.{hash}";
    }

    // بررسی اینکه رمز واردشده با هش ذخیره‌شده یکی است یا نه
    public static bool Verify(string password, string storedHash)
    {
        // هش ذخیره‌شده را به دو بخش نمک و هش اصلی جدا کن
        var parts = storedHash.Split('.');
        if (parts.Length != 2) return false;

        byte[] salt = Convert.FromBase64String(parts[0]);
        string newHash = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 100000,
            numBytesRequested: 32));

        return newHash == parts[1];
    }
}
