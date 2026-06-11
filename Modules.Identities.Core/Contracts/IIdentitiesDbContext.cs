
using Microsoft.EntityFrameworkCore;
using Modules.Identities.Core.Entities;

namespace Modules.Identities.Core.Contracts;

public interface IIdentitiesDbContext
{
    public DbSet<User> Users { get; set; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}