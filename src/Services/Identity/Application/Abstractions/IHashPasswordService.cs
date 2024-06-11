namespace Application.Abstractions;

public interface IHashPasswordService
{
    string HashPassword(string password);
    bool VerifyPassword(string password, string passwordHashed);
}