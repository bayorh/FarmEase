

using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions options): base(options)
    {
        
    }
    public DbSet<User> Users { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<LoanProduct> LoanProducts { get; set; }
    public DbSet<Loan> Loans { get; set; }
  
}
