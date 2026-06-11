
using Shared.Core.Dtos;

public interface IAuthService
{
    Task<Result<string>> Authenticate(string username, string password);
    Task<Result> Register(string username, string email, string password);
    Task<Result> GetResetPasswordTokenbyMail(string email);
    Task<Result<UserDto>> GetUserbyEmail(string email);
    Task<Result> UpdatePassword(string password, string email, string resetToken);
}
