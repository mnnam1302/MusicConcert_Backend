using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Outbox;

namespace Persistence;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(typeof(AssemblyReference).Assembly);
    }

    public DbSet<Organization> Organizations { get; set; }

    public DbSet<AppEmployee> AppEmployees { get; set; }

    public DbSet<AppCustomer> AppCustomers { get; set; }

    public DbSet<AppRole> AppRoles { get; set; }

    public DbSet<OutboxMessage> OutboxMessages { get; set; }
}