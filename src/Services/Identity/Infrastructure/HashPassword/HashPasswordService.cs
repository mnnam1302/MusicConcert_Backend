using Application.Abstractions;
using System.Security.Cryptography;

namespace Infrastructure.HashPassword;

public class HashPasswordService : IHashPasswordService
{
    public string HashPassword(string password)
    {
        string passwordHash = BCrypt.Net.BCrypt.EnhancedHashPassword(password, 10);
        return passwordHash;
    }

    public bool VerifyPassword(string password, string passwordHashed)
    {
        return BCrypt.Net.BCrypt.EnhancedVerify(password, passwordHashed);
    }
}