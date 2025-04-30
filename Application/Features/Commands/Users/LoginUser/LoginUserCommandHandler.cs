using Application.Features.Commands.Users.LoginUser;
using Domain.Contracts;
using Domain.Entities;
using Domain.Services;
using MediatR;

namespace Application.Features.Commands.Users.RegisterUser;

public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, LoginUserResponse>
{

    private readonly IAuthService _authService;
    public LoginUserCommandHandler(IAuthService authService)
    {
        _authService = authService;
    }
    public async Task<LoginUserResponse> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var response = new LoginUserResponse();

        try
        {            
            //generate token
            var token = await _authService.Authenticate(request.Email, request.Password);
            if (token != null) response.Token = token;
                
            response.Success = true;
            response.Message = "login successful";

        }
        catch (Exception ex) 
        {
            response.Success = false;
            response.Message = ex.Message;
        }
       
        return response;
    }
}
