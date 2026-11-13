namespace Fundoo.Application.Features.Authentication.Services;

public interface IEmailSender
{
    Task SendAsync(
        string recipientEmail,
        string subject,
        string htmlBody,
        CancellationToken cancellationToken);
}
