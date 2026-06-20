namespace FarmEaseApp.Models;

public sealed record LoginModel
{
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
}