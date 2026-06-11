

using Modules.Identities.Core.Entities;

namespace Modules.Identities.Core.Contracts
{
    public interface IJwtProvider
    {
        string GenerateToken(User user);
        string GeneratePasswordResetToken(User user);
        Task<bool> ValidateResetToken(string token);
    }
}
