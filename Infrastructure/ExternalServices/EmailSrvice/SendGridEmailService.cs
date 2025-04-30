
using Domain.Configs;
using Domain.Contracts;
using SendGrid.Helpers.Mail;
using SendGrid;

namespace Infrastructure.ExternalServices.EmailSrvice;

public class SendGridEmailService : IEmailService
{
    private readonly EmailSettings _settings;
    private readonly SendGridSettings _sendGridSettings;

    public SendGridEmailService(EmailSettings settings)
    {
        _settings = settings;
        _sendGridSettings = _settings.SendGrid;
    }
    public async Task SendAsync(string to, string subject, string body)
    {
        var client = new SendGridClient(_sendGridSettings.ApiKey);
        var from = new EmailAddress(_sendGridSettings.SenderEmail, _sendGridSettings.SenderName);
        var toEmail = new EmailAddress(to);
        var msg = MailHelper.CreateSingleEmail(from, toEmail, subject, null, body);
        await client.SendEmailAsync(msg);
    }
}
