

using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Shared.Core.Configs;
using Shared.Core.Contracts;

namespace Modules.Identities.Infrastructure;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly TokenDetails _detials;


    public CurrentUserService(IHttpContextAccessor contextAccessor)
    {
        _contextAccessor = contextAccessor;
        _detials = GetCurrentUserDetails(); 
    }

    public string? UserId => _detials.Id.ToString();

    public string? Email => _detials?.Email;

    public List<string>? Role => _detials.Roles;
    private TokenDetails? GetCurrentUserDetails()
    {
        var data = _contextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.UserData)?.Value;
        if (data == null) return new TokenDetails();
        var userData = JsonSerializer.Deserialize<TokenDetails>(data);
        return userData;

    }
}


