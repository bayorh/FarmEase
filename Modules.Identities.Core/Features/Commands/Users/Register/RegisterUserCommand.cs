using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Modules.Identities.Core.Contracts;
using Modules.Identities.Core.Entities;
using Shared.Core.Dtos;
using Shared.Dispatcher;

namespace Modules.Identities.Core.Features.Commands.Users.Register;

public sealed record RegisterUserCommand: IRequest<Result>
{
    public string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}
internal class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
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

public class RegisterUserCommandHandler(
    IIdentitiesDbContext context,
    IPasswordHasher passwordHasher) 
    : 
    IRequestHandler<RegisterUserCommand, Result>
{
    public async ValueTask<Result> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var user = await context.Users
            .FirstOrDefaultAsync(u => u.Username == request.Username || u.Email == request.Email);
        if (user != null)
            return Result.Failure($"User with username: {request.Username} or email: {request.Email} already exist",
            ResultStatusCode.BadRequest);
        
        user = User.Create(request.Username, request.Email, request.Password, passwordHasher);
        user.InitiatedBy = request.Email;
        await context.Users.AddAsync(user);
        await context.SaveChangesAsync();
        return Result.Success("User Registered Successfully");
   
    }
}
