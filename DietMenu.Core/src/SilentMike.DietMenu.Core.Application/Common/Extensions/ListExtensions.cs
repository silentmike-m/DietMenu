namespace SilentMike.DietMenu.Core.Application.Common.Extensions;

using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;
using System.Text.Json;

[ExcludeFromCodeCoverage]
public static class ListExtensions
{
    public static string GetHashString<T>(this IReadOnlyList<T> self)
    {
        var dataBytes = JsonSerializer.SerializeToUtf8Bytes(self);

        using var cryptoProvider = SHA256.Create();
        var hash = cryptoProvider.ComputeHash(dataBytes);
        var hashString = BitConverter.ToString(hash);

        return hashString;
    }
}
