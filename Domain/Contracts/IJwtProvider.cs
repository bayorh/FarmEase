

using Domain.Entities;
using System.Security.Claims;

namespace Domain.Contracts
{
    public interface IJwtProvider
    {
        string GenerateToken(User user);
        string GeneratePasswordResetToken(User user);
        Task<bool> ValidateResetToken(string token);
    }
}
