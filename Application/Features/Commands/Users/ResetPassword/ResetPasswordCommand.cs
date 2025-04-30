using MediatR;

namespace Application.Features.Commands.Users.ResetPassword;

public record ResetPasswordCommand: IRequest<ResetPasswordCommandResponse>
{
    public string Email { get; set; }
    public string ResetToken {  get; set; }
    public string Password { get; set; }
}
