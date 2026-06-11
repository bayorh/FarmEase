
namespace Shared.Core.Configs;

public sealed record TokenDetails
{

    public string Email { get; set; } = default!;
    public Guid Id { get; set; } = default!;
    public string Username { get; set; } = default!;
    public List<string> Roles { get; set; } = default!;
}