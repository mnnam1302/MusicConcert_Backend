using Application.Abstractions;
using AutoMapper.Internal;
using Infrastructure.DependencyInjection.Options;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Infrastructure.Notification;

public class EmailService : IEmailService
{
    private readonly EmailOptions _emailOptions;

    public EmailService(IOptions<EmailOptions> emailOptions)
    {
        _emailOptions = emailOptions.Value;
    }

    public async Task SendEmailAsync(string email, string subject, string messageBody, CancellationToken cancellationToken = default)
    {
        var message = new MimeMessage();

        message.From.Add(new MailboxAddress(_emailOptions.DisplayName, _emailOptions.From));
        message.To.Add(MailboxAddress.Parse(email));
        message.Subject = subject;

        //message.Body = new TextPart("plain")
        //{
        //    Text = messageBody
        //};
        var builder = new BodyBuilder();
        builder.HtmlBody = $"{messageBody} \n" +
                           $"Music Concert xin cảm ơn quý khách hàng đã tin tưởng.";
        message.Body = builder.ToMessageBody();

        using var client = new SmtpClient();
        await client.ConnectAsync(_emailOptions.Host, _emailOptions.Port, SecureSocketOptions.StartTls);
        await client.AuthenticateAsync(_emailOptions.Email, _emailOptions.Password);

        await client.SendAsync(message);
        client.Disconnect(true);
    }
}