

namespace Shared.Core.Dtos;

public sealed record UserDto
{
    public Guid Id { get; set; } = default!;
    public string UserName { get; set; } = default!;
    public string Email { get; set; } = default!;
}
