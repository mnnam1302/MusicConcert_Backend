﻿using Application.Abstractions;
using System.Security.Cryptography;

namespace Infrastructure.HashPassword;

public class HashPasswordService : IHashPasswordService
{
    private readonly int keySize = 64;
    private readonly int iteration = 1000;
    private readonly HashAlgorithmName passwordHashAlgorithm = HashAlgorithmName.SHA512;

    public string GenerateSalt()
    {
        using var rng = RandomNumberGenerator.Create();

        var saltBytes = new byte[keySize];
        rng.GetBytes(saltBytes);

        return Convert.ToBase64String(saltBytes);
    }

    public string HashPassword(string password, string salt)
    {
        var saltPassword = Convert.FromBase64String(salt);

        var passwordHash = Rfc2898DeriveBytes.Pbkdf2(
                password,
                saltPassword,
                iteration,
                passwordHashAlgorithm,
                keySize);

        return Convert.ToBase64String(passwordHash);
    }

    public bool VerifyPassword(string plaintextPassword, string ciphertextPassword, string salt)
    {
        var passwordSalt = Convert.FromBase64String(salt);

        var passwordHash = Rfc2898DeriveBytes.Pbkdf2(
                plaintextPassword,
                passwordSalt,
                iteration,
                passwordHashAlgorithm,
                keySize);

        var passwordHashString = Convert.ToBase64String(passwordHash);

        return passwordHashString.Equals(ciphertextPassword);
    }
}