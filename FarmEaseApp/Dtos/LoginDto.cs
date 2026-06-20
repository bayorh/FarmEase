namespace FarmEaseApp.Dtos;

public sealed record LoginDto
{
    public string Token { get; set; } = default!;
}
