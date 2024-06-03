using Microsoft.EntityFrameworkCore;

namespace Persistence;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }

    public DbSet<Domain.Entities.Invoice> Invoices { get; set; }
    public DbSet<Domain.Entities.CustomerInfo> CustomerInfo { get; set; }
    public DbSet<Domain.Entities.OrderInfo> OrderInfo { get; set; }
}