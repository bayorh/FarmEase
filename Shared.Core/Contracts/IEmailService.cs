namespace Modules.Identities.Core.Contracts;

public interface IEmailService
{
    Task SendAsync(string to, string subject, string body);
}
