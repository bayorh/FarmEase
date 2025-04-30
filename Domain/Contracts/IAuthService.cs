

using Domain.Entities;

namespace Domain.Contracts;

public interface IAuthService
{
    Task<string> Authenticate(string username, string password);
    Task Register(string username, string email, string password);
    Task GetResetPasswordTokenbyMail(string email);
    Task<User> GetUserbyEmail(string email);
    Task UpdatePassword(string password, string email, string resetToken);
}
