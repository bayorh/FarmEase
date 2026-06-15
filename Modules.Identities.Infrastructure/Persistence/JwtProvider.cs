using Modules.Identities.Core.Contracts;
using Modules.Identities.Core.Entities;

namespace Modules.Identities.Infrastructure.Persistence;

public class JwtProvider : IJwtProvider
{
    public string GeneratePasswordResetToken(User user)
    {
        throw new NotImplementedException();
    }

    public string GenerateToken(User user)
    {
        throw new NotImplementedException();
    }

    public Task<bool> ValidateResetToken(string token)
    {
        throw new NotImplementedException();
    }
    
}