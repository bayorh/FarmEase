

namespace Domain.Contracts.Repositories;

public interface ICurrentUserService
{
    string? UserId { get; }
    string? Email { get; }
    List<string>? Role { get; }
}
