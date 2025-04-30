using FluentValidation;

namespace Application.Features.Commands.Users.RegisterUser;

public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator()
    {
        RuleFor(x => x.Username)
            .NotNull()
            .NotEmpty().WithMessage("Username cannot be empty")
            .MinimumLength(3).WithMessage("Username length must  be more than 2");
        RuleFor(x => x.Email)
            .NotNull()
            .NotEmpty().WithMessage("Email cannot be empty")
            .EmailAddress().WithMessage("Enter a valid Email type");
        RuleFor(x => x.Password).NotNull().NotEmpty().WithMessage("Password cannot be empty")
            .MinimumLength(8).WithMessage("passward length must not be less than 8");
    }
}
