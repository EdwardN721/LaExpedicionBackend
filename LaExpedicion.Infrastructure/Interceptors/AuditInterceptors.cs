using LaExpedicion.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace LaExpedicion.Infrastructure.Interceptors;

public class AuditInterceptors : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default
    )
    {
        if (eventData.Context is not null)
        {
            UpdateAuditEntities(eventData.Context);
        }
        
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    public override InterceptionResult<int> SavingChanges(
        DbContextEventData eventData,
        InterceptionResult<int> result)
    {
        if (eventData.Context is not null)
        {
            UpdateAuditEntities(eventData.Context);
        }
        
        return base.SavingChanges(eventData, result);
    }

    private static void UpdateAuditEntities(DbContext context)
    {
        DateTime now = DateTime.UtcNow;

        foreach (var entry in context.ChangeTracker.Entries<BaseEntity>())
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.FechaCreacion = now;
                entry.Entity.Activo = true;
                entry.Entity.Eliminado = false;
            }
            else if (entry.State == EntityState.Modified)
            {
                entry.Entity.FechaModificacion = now;
            }
            else if (entry.State == EntityState.Deleted)
            {
                entry.State = EntityState.Modified;
                entry.Entity.Activo = false;
                entry.Entity.Eliminado = true;
                entry.Entity.FechaModificacion = now;
            }
        }
    }
}