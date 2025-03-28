using System.Security.Cryptography;
using System.Text;

namespace Domain.Extensions;

public static class UrlExtensions
{
    public static string ToBase64Prefix(this string longUrl)
    {
        var hash = SHA256.HashData(Encoding.UTF8.GetBytes(longUrl));
        return Convert.ToBase64String(hash)[..7];
    }
}