

namespace Shared.Core.Contracts;

public interface ICurrentUserService
{
    string? UserId { get; }
    string? Email { get; }
    List<string>? Role { get; }
}
