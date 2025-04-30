

namespace Domain.Entities;

public abstract class AuditableEntity
{
    public string InitiatedBy { get; set; }
    public string? ModifiedBy { get; set; }
    public DateTime CreatedAt { get; protected set; }
    public DateTime? ModifiedAt { get; protected set; }

    public void SetCreated(string userId)
    {
        CreatedAt = DateTime.UtcNow;
        InitiatedBy = userId;
    }

    public void SetModified(string userId)
    {
        ModifiedAt = DateTime.UtcNow;
        ModifiedBy = userId;

    }
}
