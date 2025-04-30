using Domain.Shared;

namespace Application.Features.Commands.Users.LoginUser;

public record LoginUserResponse : BaseResponse
{
    public string Token { get; set; }
}