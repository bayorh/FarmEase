
using Domain.Configs;
using Domain.Contracts;
using Domain.Entities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace Infrastructure.ExternalServices;

public class JwtProvider : IJwtProvider
{
    private readonly Jwt _jwt;
    public JwtProvider(IOptions<Jwt> jwtsettings, ILogger<JwtProvider> logger)
    {
        _jwt = jwtsettings.Value;
    }

    public string GeneratePasswordResetToken(User user)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim("reset", "true"),
        };
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _jwt.Issuer,
            audience: _jwt.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(10), // short expiry
            signingCredentials: creds
            );
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
    public string GenerateToken(User user)
    {
        var userdata = new TokenDetails()
        {
           Email = user.Email,
           Id = user.Id,
           Username = user.Username,
           Roles = user.Roles
        };

        var claims = new[]
        {
            new Claim(ClaimTypes.UserData, UserDataSeriliser(userdata)),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Role,string.Join(",",user.Roles))
        };
        foreach (var role in user.Roles)
        {
            new Claim(ClaimTypes.Role, role);   
        }
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _jwt.Issuer,
            audience: _jwt.Audience,
            claims: claims,
            expires: DateTime.Now.AddHours(2),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);

    }

    public async Task<bool> ValidateResetToken(string token)
    {
        var tokenhandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_jwt.Key);

        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,

            ValidIssuer = _jwt.Issuer,
            ValidAudience = _jwt.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ClockSkew = TimeSpan.Zero,

        };
                
        var validationResult =  await tokenhandler.ValidateTokenAsync(token,validationParameters);
        if (!validationResult.IsValid) return false;
        if (validationResult.ClaimsIdentity.HasClaim(c => c.Type == "reset" && c.Value == "true")) return true;
        return false;
    }
    private string UserDataSeriliser(TokenDetails details) =>
            JsonSerializer.Serialize(details).ToString();
}

