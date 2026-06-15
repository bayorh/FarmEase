using Modules.Identities.Core.Contracts;

namespace Modules.Identities.Infrastructure.Helpers;

public class PasswordHasher : IPasswordHasher
{
    public string Hash(string password)
    {
        throw new NotImplementedException();
    }

    public bool Verify(string hash, string password)
    {
        throw new NotImplementedException();
    }
}