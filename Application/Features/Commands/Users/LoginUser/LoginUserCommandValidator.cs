
using FluentValidation;

namespace Application.Features.Commands.Users.LoginUser;

public class LoginUserCommandValidator : AbstractValidator<LoginUserCommand>
{
    public LoginUserCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotNull()
            .NotEmpty().WithMessage("Username cannot be empty")
            .EmailAddress().WithMessage("enter a valid email to proceed");
        RuleFor(x => x.Password).NotNull().NotEmpty().WithMessage("Password cannot be empty")
            .MinimumLength(8).WithMessage("passward length must not be less than 8");
    }
}
