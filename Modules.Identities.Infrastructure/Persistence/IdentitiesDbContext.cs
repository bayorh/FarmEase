using Microsoft.EntityFrameworkCore;
using Modules.Identities.Core.Contracts;
using Modules.Identities.Core.Entities;
using Shared.Infrastructure.Persistence;

namespace Modules.Identities.Infrastructure.Persistence;

public class IdentitiesDbContext(DbContextOptions<IdentitiesDbContext> options)
    : ModuleDbContext(options), IIdentitiesDbContext
{
    protected override string Schema => "Identities";
    public DbSet<User> Users { get; set; } 

}