namespace Application.Abstractions;

public interface IHashPasswordService
{
    string HashPassword(string password, string salt);
    bool VerifyPassword(string plaintextPassword, string ciphertextPassword, string salt);
    string GenerateSalt();
}