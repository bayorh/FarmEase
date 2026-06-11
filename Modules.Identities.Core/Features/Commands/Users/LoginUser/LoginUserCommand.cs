

using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Modules.Identities.Core.Contracts;
using Shared.Core.Dtos;
using Shared.Dispatcher;

namespace Modules.Identities.Core.Features.Commands.Users.LoginUser;

public sealed record LoginUserCommand(
    string Email,
    string Password) : IRequest<Result<LoginUserResponseDto>>;

public class LoginUserCommandValidator : 
    AbstractValidator<LoginUserCommand>
{
    public LoginUserCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotNull()
            .NotEmpty().WithMessage("Username cannot be empty")
            .EmailAddress().WithMessage("enter a valid email to proceed");
        RuleFor(x => x.Password).NotNull()
            .NotEmpty().WithMessage("Password cannot be empty")
            .MinimumLength(8).WithMessage("passward length must not be less than 8");
    }
}
public class LoginUserCommandHandler(
    IIdentitiesDbContext context,
    IPasswordHasher passwordHasher,
    IJwtProvider jwtProvider)
    : IRequestHandler<LoginUserCommand, Result<LoginUserResponseDto>>
{
   
    public async  ValueTask<Result<LoginUserResponseDto>> Handle(LoginUserCommand request, 
        CancellationToken cancellationToken)
    {
    
        var user = await context.Users
            .FirstOrDefaultAsync(u => u.Username == request.Email || u.Email == request.Email);
        if (user == null)
            return Result<LoginUserResponseDto>.Failure($"User with {request.Email} not found",
            ResultStatusCode.BadRequest);

        if (!user.VerifyPassword(request.Password, passwordHasher))
            return Result<LoginUserResponseDto>.Failure("Invalid Password",
            ResultStatusCode.BadRequest);

        return Result<LoginUserResponseDto>.Success(new LoginUserResponseDto
        {
            Token = jwtProvider.GenerateToken(user)
        });
    }
}