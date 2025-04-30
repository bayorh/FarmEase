using Domain.Contracts;
using MediatR;

namespace Application.Features.Commands.Users.ResetPasswordlink;

public class SendResetPasswordlinkToMailRequestHandler : IRequestHandler<SendResetPasswordlinkToMailRequest, SendResetPasswordlinkToMailResponse>
{
    private readonly IAuthService _authService;
    public SendResetPasswordlinkToMailRequestHandler(IAuthService authService)
    {
        _authService = authService;
    }
    public async Task<SendResetPasswordlinkToMailResponse> Handle(SendResetPasswordlinkToMailRequest request, CancellationToken cancellationToken)
    {
        var response = new SendResetPasswordlinkToMailResponse();
        try
        {
            await _authService.GetResetPasswordTokenbyMail(request.Email);
            response.Success = true;
            response.Message = $"Reset token sent to {request.Email} ";
        }
        catch (Exception ex)
        {
            response.Success = false;
            response.Message = ex.Message;
        }
        return response;
    }
}
