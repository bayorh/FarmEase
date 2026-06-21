using System.Net.Http.Json;
using System.Text.Json;
using FarmEaseApp.Dtos;
using FarmEaseApp.Models;

namespace FarmEaseApp.Services.Auth;

public class AuthService (HttpClient httpClient): IAuthService
{
    private readonly JsonSerializerOptions _jsonOptions =
         new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

    public async Task<Result<LoginDto>> LoginAsync(LoginModel request)
    {
        try
        {
            var response = await httpClient.PostAsJsonAsync("api/auth/login", request);
            var contentString = await response.Content.ReadAsStringAsync();

            if (!string.IsNullOrEmpty(contentString))
            {
                var apiResult = JsonSerializer.Deserialize<Result<LoginDto>>(contentString, _jsonOptions);
                if (apiResult != null) return apiResult;
            }

            return new Result<LoginDto>
            {
                IsSuccess = false,
                Message = $"Server error: {response.StatusCode}",
                StatusCode = (int)response.StatusCode
            };
        }
        catch (Exception ex)
        {
            return new Result<LoginDto>
            {
                IsSuccess = false,
                Message = $"An unexpected system error occurred: {ex.Message}",
                StatusCode = 500
            };
        }
    }

    public async Task<Result> RegisterAsync(RegisterRequest request)
    {
        try
        {
            var response = await httpClient.PostAsJsonAsync("api/auth/register", request);
            var contentString = await response.Content.ReadAsStringAsync();

            if (!string.IsNullOrEmpty(contentString))
            {
                var apiResult = JsonSerializer.Deserialize<Result>(contentString, _jsonOptions);
                if (apiResult != null) return apiResult;
            }

            return new Result
            {
                IsSuccess = false,
                Message = $"Server error: {response.StatusCode}",
                StatusCode = (int)response.StatusCode
            };
        }
        catch (Exception ex)
        {
            return new Result
            {
                IsSuccess = false,
                Message = $"An unexpected system error occurred: {ex.Message}",
                StatusCode = 500
            };
        }
    }
}