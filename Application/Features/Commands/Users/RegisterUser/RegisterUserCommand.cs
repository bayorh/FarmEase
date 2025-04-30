using MediatR;

namespace Application.Features.Commands.Users.RegisterUser;

public record RegisterUserCommand: IRequest<RegisterUserCommandResponse>
{
    public string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}
