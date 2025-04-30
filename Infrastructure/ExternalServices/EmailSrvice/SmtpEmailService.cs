
using Domain.Configs;
using Domain.Contracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace Infrastructure.ExternalServices.EmailSrvice;

public class SmtpEmailService : IEmailService
{

    private readonly SmtpSettings _smtpSettigs;
    
    public SmtpEmailService(IOptions<SmtpSettings> settings)
    {

        _smtpSettigs = settings.Value;
    }

    public async Task SendAsync(string to, string subject, string body)
    {
        var message = new MailMessage
        {
            From = new MailAddress(_smtpSettigs.SenderEmail, _smtpSettigs.SenderName),
            Subject = subject,
            Body = body,
            IsBodyHtml = true
        };
        message.To.Add(new MailAddress(to));
        using var client = new SmtpClient(_smtpSettigs.SmtpServer, _smtpSettigs.Port)
        {
            Credentials = new NetworkCredential(_smtpSettigs.Username, _smtpSettigs.Password),
            EnableSsl = true
        };
        await client.SendMailAsync(message);
    }
}
