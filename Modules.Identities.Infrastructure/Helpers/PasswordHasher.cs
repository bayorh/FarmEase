using Microsoft.AspNetCore.Identity;
using Modules.Identities.Core.Contracts;

namespace Modules.Identities.Infrastructure.Helpers;

public class PasswordHasher : IPasswordHasher
{
    private readonly PasswordHasher<object> _hasher = new();

    public string Hash(string password)
    {
        return _hasher.HashPassword(null, password);
    }

    public bool Verify(string hash, string password)
    {
        return _hasher.VerifyHashedPassword(null, hash, password) != PasswordVerificationResult.Failed;
    }
}