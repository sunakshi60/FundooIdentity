using Fundoo.Application.Features.Authentication.Services;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Fundoo.Infrastructure.Email;

public sealed class SmtpEmailSender(IOptions<SmtpOptions> options) : IEmailSender
{
    public async Task SendAsync(
        string recipientEmail,
        string subject,
        string htmlBody,
        CancellationToken cancellationToken)
    {
        var smtp = options.Value;

        if (string.IsNullOrWhiteSpace(smtp.Host)
            || string.IsNullOrWhiteSpace(smtp.UserName)
            || string.IsNullOrWhiteSpace(smtp.Password)
            || string.IsNullOrWhiteSpace(smtp.FromAddress)
            || smtp.Port is < 1 or > 65535)
        {
            throw new InvalidOperationException(
                "SMTP settings are incomplete. Configure the Smtp section before sending email.");
        }

        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(smtp.FromName, smtp.FromAddress));
        message.To.Add(MailboxAddress.Parse(recipientEmail));
        message.Subject = subject;
        message.Body = new BodyBuilder { HtmlBody = htmlBody }.ToMessageBody();

        using var client = new SmtpClient();
        await client.ConnectAsync(
            smtp.Host,
            smtp.Port,
            SecureSocketOptions.StartTls,
            cancellationToken);
        await client.AuthenticateAsync(smtp.UserName, smtp.Password, cancellationToken);
        await client.SendAsync(message, cancellationToken);
        await client.DisconnectAsync(true, cancellationToken);
    }
}
