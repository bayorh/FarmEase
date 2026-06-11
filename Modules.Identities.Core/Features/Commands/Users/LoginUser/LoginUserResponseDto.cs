
namespace Modules.Identities.Core.Features.Commands.Users.LoginUser;

public record LoginUserResponseDto
{
    public string Token { get; set; } = default!;
}