

using FluentValidation;

namespace Application.Features.Commands.Users.ResetPassword;

public  class ResetPasswordCommandValidator : AbstractValidator<ResetPasswordCommand>
{
    public ResetPasswordCommandValidator()
    {
        RuleFor(x => x.Email)
           .NotNull()
           .NotEmpty().WithMessage("Email cannot be empty")
           .EmailAddress().WithMessage("Enter a valid Email type");
        RuleFor(x => x.ResetToken)
          .NotNull()
          .NotEmpty().WithMessage("Email cannot be empty");
        RuleFor(x => x.Password)
           .NotNull()
           .NotEmpty().WithMessage("Email cannot be empty")
           .MinimumLength(8).WithMessage("Password must not be less than 8 characters");
    }
}
