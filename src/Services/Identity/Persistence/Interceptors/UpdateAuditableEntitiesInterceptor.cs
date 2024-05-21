using Domain.Abstractions.Entities;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Interceptors;

public class UpdateAuditableEntitiesInterceptor : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
    DbContextEventData eventData,
    InterceptionResult<int> result,
    CancellationToken cancellationToken = default)
    {
        DbContext? dbContext = eventData.Context;

        if (dbContext is null)
        {
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        IEnumerable<EntityEntry<IAuditable>> entites =
            dbContext.ChangeTracker
            .Entries<IAuditable>();

        foreach (EntityEntry<IAuditable> entityEntry in entites)
        {
            if (entityEntry.State == EntityState.Added)
            {
                entityEntry.Property(x => x.CreatedOnUtc).CurrentValue = DateTime.UtcNow;
            }

            if (entityEntry.State == EntityState.Modified)
            {
                entityEntry.Property(x => x.ModifiedOnUtc).CurrentValue = DateTime.UtcNow;
            }
        }

        IEnumerable<EntityEntry<ISoftDeleted>> softDeleteEntities =
            dbContext.ChangeTracker
            .Entries<ISoftDeleted>();

        foreach (EntityEntry<ISoftDeleted> entityEntry in softDeleteEntities)
        {
            if (entityEntry.State == EntityState.Deleted)
            {
                entityEntry.State = EntityState.Modified;
                entityEntry.Property(x => x.IsDeleted).CurrentValue = true;
                entityEntry.Property(x => x.DeletedOnUtc).CurrentValue = DateTime.UtcNow;
            }
        }

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}