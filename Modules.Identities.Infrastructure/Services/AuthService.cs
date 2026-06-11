
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Modules.Identities.Core.Contracts;
using Modules.Identities.Core.Entities;
using Shared.Core.Dtos;


namespace Modules.Identities.Infrastructure.Services;

public class AuthService(
    IIdentitiesDbContext context,
    IJwtProvider jwtProvider,
    IPasswordHasher passwordHasher,
    IEmailService emailService,
    IConfiguration config,
    ILogger<AuthService> logger) : IAuthService
{
  



    public async Task<ServiceResult> GetResetPasswordTokenbyMail(string email)
    {
        var user = await context.Users.FirstOrDefaultAsync(u => u.Email == email);
        if (user == null)
            return new ServiceResult
            {
                IsSuccess = false,
                Message = $"User with {email} not found"
            };
        var token = jwtProvider.GeneratePasswordResetToken(user);
        logger.LogInformation(token); //to be modified
        var resetLink = $"{config["Frontend:ResetPasswordUrl"]}?token={token}";
        await emailService.SendAsync(user.Email, "Reset Your Password", $"Click to reset: {resetLink}");
        return new ServiceResult
        {
            IsSuccess = true,
            Message = "Password reset link sent successfully"
        };
    }

    public async Task<ServiceResult> UpdatePassword(string password, string email, string resetToken)
    {
       
    }


