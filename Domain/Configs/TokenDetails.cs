
namespace Domain.Configs;

public class TokenDetails
{
    public TokenDetails()
    {
    }

    public string Email { get; set; }
    public Guid Id { get; set; }
    public string Username { get; set; }
    public List<string> Roles { get; set; }
}