using Domain.Contracts;
using MediatR;

namespace Application.Features.Commands.Users.ResetPassword;

public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, ResetPasswordCommandResponse>
{
    private readonly IAuthService _authService;
    public ResetPasswordCommandHandler(IAuthService authService)
    {
        _authService = authService;
    }
    public async Task<ResetPasswordCommandResponse> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        var response = new ResetPasswordCommandResponse();
        try
        {
            _authService.UpdatePassword(request.Password,request.Email,request.ResetToken);
            response.Success = true;
            response.Message = "Updated successfully";
        }
        catch (Exception ex) 
        {
            response.Success = false;
            response.Message = ex.Message;
        };
        return response;
    }
}