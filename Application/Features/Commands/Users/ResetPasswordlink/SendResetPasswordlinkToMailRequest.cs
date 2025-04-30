using MediatR;

namespace Application.Features.Commands.Users.ResetPasswordlink;

public class SendResetPasswordlinkToMailRequest : IRequest<SendResetPasswordlinkToMailResponse>
{
    public string Email { get; set; }
}
