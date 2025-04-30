

using Domain.Contracts;
using MediatR;

namespace Application.Features.Commands.Users.RegisterUser;

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, RegisterUserCommandResponse>
{
    private readonly IAuthService _authService;
    public RegisterUserCommandHandler(IAuthService authService)
    {
        _authService = authService;
    }
    public async  Task<RegisterUserCommandResponse> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var response = new RegisterUserCommandResponse();
        try
        {
            await _authService.Register(request.Username,request.Email,request.Password);
            
            response.Success = true;
            response.Message = "Registered successfully";
        }
        catch(Exception ex)
        {
            response.Success = false;
            response.Message = ex.Message;
        }
        return response;
    }
}
