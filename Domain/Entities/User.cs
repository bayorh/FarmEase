

using Domain.Contracts;

namespace Domain.Entities;

public partial class User: BaseEntity
{
    public string Username { get; private set; }
    public string Email { get; private set; }
    public string PasswordHash { get; private set; }
    

    public List<string> Roles { get; private set; } = new();
    public User()
    {
        
    }


    public static User Create(string username, string email, string password, IPasswordHasher hasher)
    {
        var user = new User
        {
            Username = username,
            Email = email,            
        };
        user.SetPassword(password, hasher);
        user.SetCreated(user.Id.ToString());
        user.Roles.Add(RoleType.User.ToString()); // Default role
        return user;
    }
    public User UpdatePassword(string password, IPasswordHasher passwordHasher)
    {
        
        PasswordHash = passwordHasher.Hash(password);
        SetModified(this.Id.ToString());
        return this;
    }
    private void SetPassword(string password, IPasswordHasher passwordHasher)
    {
        PasswordHash = passwordHasher.Hash(password);
    }

    public bool VerifyPassword(string password, IPasswordHasher passwordHasher)
    {
        return passwordHasher.Verify(PasswordHash, password);
    }
}
