using System.Net.Http.Json;
using FarmEaseApp.Dtos;
using FarmEaseApp.Models;

namespace FarmEaseApp.Services.Auth;

public class AuthService (HttpClient httpClient): IAuthService
{
    public async Task<Result> RegisterAsync(RegisterRequest request)
    {
        try
        {
            var response = await httpClient.PostAsJsonAsync("api/auth/register", request);

            if (response.IsSuccessStatusCode)
            {
                return new Result { IsSuccess = true };
            }

            var errorContent = await response.Content.ReadAsStringAsync();
            return new Result() 
            { 
                IsSuccess = false, 
                ErrorMessage = !string.IsNullOrEmpty(errorContent) ? errorContent : "Registration failed. Server error." 
            };
        }
        catch (HttpRequestException)
        {
            return new resu { IsSuccess = false, ErrorMessage = "Network error: Connection refused." };
        }
    }
}