
namespace Modules.Identities.Core.Entities;

public class BaseEntity: AuditableEntity
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public BaseEntity()
    {
        
    }
    public BaseEntity(Guid id)
    {
        Id = id;
    }
}
