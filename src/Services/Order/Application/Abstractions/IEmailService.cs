namespace Application.Abstractions;

public interface IEmailService
{
    Task SendEmailAsync(string email, string subject, string messageBody, CancellationToken cancellationToken = default);
}