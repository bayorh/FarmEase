using FluentValidation;

namespace Application.Features.Commands.Users.ResetPasswordlink;

public class SendResetPasswordlinkToMailRequestValidator : AbstractValidator<SendResetPasswordlinkToMailRequest>
{
    public SendResetPasswordlinkToMailRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotNull()
            .NotEmpty().WithMessage("Email cannot be empty")
            .EmailAddress().WithMessage("Enter a valid Email type");
    }
}

