using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using System.Text.Json;
using Microsoft.JSInterop;
namespace FarmEaseApp.AuthProviders;

public class AuthStateProvider(IJSRuntime jsRuntime) : AuthenticationStateProvider
{
    private readonly ClaimsPrincipal _anonymous = new(new ClaimsIdentity());

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        try
        {
            var token = await jsRuntime.InvokeAsync<string>("localStorage.getItem", "authToken");

            if (string.IsNullOrEmpty(token))
                return new AuthenticationState(_anonymous);

            var identity = new ClaimsIdentity(ParseClaimsFromJwt(token), "jwt");
            var user = new ClaimsPrincipal(identity);

            // TODO: Add validation here to check if token is expired!
            
            return new AuthenticationState(user);
        }
        catch
        {
            return new AuthenticationState(_anonymous);
        }
    }
    public async Task MarkUserAsAuthenticated(string token)
    {
        // Save the token to local storage so it persists across page refreshes
        await jsRuntime.InvokeVoidAsync("localStorage.setItem", "authToken", token);
        var identity = new ClaimsIdentity(ParseClaimsFromJwt(token), "jwt");
        var authUser = new ClaimsPrincipal(identity);
        var authStateTask = Task.FromResult(new AuthenticationState(authUser));
        
        NotifyAuthenticationStateChanged(authStateTask);
    }
    public async Task MarkUserAsLoggedOut()
    {
        // remove the token from local storage 
        await jsRuntime.InvokeVoidAsync("localStorage.removeItem", "authToken");
        var authStateTask = Task.FromResult(new AuthenticationState(_anonymous));
        NotifyAuthenticationStateChanged(authStateTask);
    }
    private IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
    {
        var claims = new List<Claim>();
        var payload = jwt.Split('.')[1];

        // Normalize base64 string padding
        switch (payload.Length % 4)
        {
            case 2: payload += "=="; break;
            case 3: payload += "="; break;
        }

        var jsonBytes = Convert.FromBase64String(payload);
        var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);

        if (keyValuePairs != null)
        {
            foreach (var kvp in keyValuePairs)
            {
                if (kvp.Value is JsonElement jsonElement && jsonElement.ValueKind == JsonValueKind.Array)
                {
                    foreach (var element in jsonElement.EnumerateArray())
                    {
                        claims.Add(new Claim(kvp.Key, element.ToString()));
                    }
                }
                else
                {
                    claims.Add(new Claim(kvp.Key, kvp.Value.ToString() ?? string.Empty));
                }
            }
        }
        return claims;
    }
}