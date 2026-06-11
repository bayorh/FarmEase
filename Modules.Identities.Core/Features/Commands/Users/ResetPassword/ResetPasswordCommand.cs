
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Modules.Identities.Core.Contracts;
using Shared.Core.Dtos;
using Shared.Dispatcher;

namespace Modules.Identities.Core.Features.Commands.Users.ResetPassword;

public record ResetPasswordCommand: IRequest<Result>
{
    public string Email { get; set; }
    public string ResetToken {  get; set; }
    public string Password { get; set; }
}
public class ResetPasswordCommandValidator : AbstractValidator<ResetPasswordCommand>
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
public class ResetPasswordCommandHandler(
    IIdentitiesDbContext context,
    IJwtProvider jwtProvider,
    IPasswordHasher passwordHasher
)
    : IRequestHandler<ResetPasswordCommand, Result>
{
    public async ValueTask<Result> Handle(ResetPasswordCommand request,
        CancellationToken cancellationToken)
    {
      
         var user = await context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
        if (user == null)
            return Result.Failure($"User with Email: {request.Email} not found",
            ResultStatusCode.BadRequest);
           

        //validate reset token
        var validationResult = await jwtProvider.ValidateResetToken(request.ResetToken);
        if (!validationResult)
            return Result.Failure("Invalid or expired validation token.",
            ResultStatusCode.BadRequest);

        var _user = user.UpdatePassword(request.Password, passwordHasher);
        context.Users.Update(_user);
        await context.SaveChangesAsync();
        return Result.Success("Password updated successfully");

    }
}