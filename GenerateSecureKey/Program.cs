using System;
using System.Security.Cryptography;
using System.Text;

public class Program
{
    public static void Main()
    {
        var key = GenerateSecureKey();
        Console.WriteLine(key);
    }

    private static string GenerateSecureKey()
    {
        using (var rngCryptoServiceProvider = new RNGCryptoServiceProvider())
        {
            var randomBytes = new byte[32]; // 256 bits
            rngCryptoServiceProvider.GetBytes(randomBytes);
            return Convert.ToBase64String(randomBytes);
        }
    }
}