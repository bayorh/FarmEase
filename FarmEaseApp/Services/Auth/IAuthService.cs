using FarmEaseApp.Dtos;
using FarmEaseApp.Models;

namespace FarmEaseApp.Services.Auth;

public interface IAuthService
{
    Task<Result> RegisterAsync(RegisterRequest request);
    Task<Result<LoginDto>> LoginAsync(LoginModel request);
}