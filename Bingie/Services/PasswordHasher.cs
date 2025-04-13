using System.Security.Cryptography;
using System.Text;

namespace Bingie.Services;

public class PasswordHasher
{
    private const int SaltSize = 16; // 128 bits
    private const int KeySize = 32; // 256 bits
    private const int Iterations = 10000;
    private static readonly HashAlgorithmName Algorithm = HashAlgorithmName.SHA256;
    private const char Delimiter = ':';

    public string Hash(string password)
    {
        var salt = RandomNumberGenerator.GetBytes(SaltSize);
        var hash = Rfc2898DeriveBytes.Pbkdf2(
            Encoding.UTF8.GetBytes(password),
            salt,
            Iterations,
            Algorithm,
            KeySize);

        return string.Join(
            Delimiter,
            Convert.ToBase64String(salt),
            Convert.ToBase64String(hash),
            Iterations,
            Algorithm);
    }

    public bool Verify(string passwordHash, string inputPassword)
    {
        var elements = passwordHash.Split(Delimiter);
        var salt = Convert.FromBase64String(elements[0]);
        var hash = Convert.FromBase64String(elements[1]);
        var iterations = int.Parse(elements[2]);
        var algorithm = new HashAlgorithmName(elements[3]);

        var inputHash = Rfc2898DeriveBytes.Pbkdf2(
            Encoding.UTF8.GetBytes(inputPassword),
            salt,
            iterations,
            algorithm,
            hash.Length);

        return CryptographicOperations.FixedTimeEquals(hash, inputHash);
    }
}
