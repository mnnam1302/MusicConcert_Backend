using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.OutboxMessages;

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

    public DbSet<OrganizationInfo> OrganizationInfos { get; set; }

    public DbSet<Category> Categories { get; set; }
    public DbSet<Event> Events { get; set; }
    public DbSet<Ticket> Tickets { get; set; }

    public DbSet<OutboxMessage> OutboxMessages { get; set; }
}