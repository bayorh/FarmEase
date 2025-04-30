

namespace Domain.Configs;

public class EmailSettings
{
    public SmtpSettings Smtp { get; set; }
    public SendGridSettings SendGrid { get; set; }
}

public class SendGridSettings
{
    public string ApiKey { get; set; }
    public string SenderEmail { get; set; }
    public string SenderName { get; set; }
}

public class SmtpSettings
{
    public string SmtpServer { get; set; }
    public int Port { get; set; }
    public string SenderEmail { get; set; }
    public string SenderName { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
}