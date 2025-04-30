using MediatR;

namespace Application.Features.Commands.Users.LoginUser;

public class LoginUserCommand : IRequest<LoginUserResponse>
{
    public string Email { get; set; }
    public string Password { get; set; }

}
