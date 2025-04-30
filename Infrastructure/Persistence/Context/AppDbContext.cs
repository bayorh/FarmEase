

using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions options): base(options)
    {
        
    }
    public DbSet<User> Users { get; set; }
  
}
