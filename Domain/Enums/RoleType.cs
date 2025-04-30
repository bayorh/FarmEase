namespace Domain.Entities;

public partial class User
{
    public enum RoleType
    {
        User = 1,
        Manger,
        Admin
    }
}
