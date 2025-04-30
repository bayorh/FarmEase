namespace Application.Features.Commands.Users.ResetPassword;

public record ResetPasswordCommandResponse: BaseResponse
{
    public string Token { get; set; }
}
