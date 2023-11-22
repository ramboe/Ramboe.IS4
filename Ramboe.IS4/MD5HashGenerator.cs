using System.Security.Cryptography;
using System.Text;

namespace Ramboe.IS4;

public static class MD5HashGenerator
{
    public static string CalculateMD5Hash(this string input)
    {
        // Convert the input string to a byte array and compute the hash.
        using var md5 = MD5.Create();

        var inputBytes = Encoding.UTF8.GetBytes(input);
        var hashBytes = md5.ComputeHash(inputBytes);

        // Convert the byte array to a hexadecimal string.
        var sb = new StringBuilder();

        for (var i = 0; i < hashBytes.Length; i++)
        {
            sb.Append(hashBytes[i].ToString("x2"));
        }

        return sb.ToString();
    }
}