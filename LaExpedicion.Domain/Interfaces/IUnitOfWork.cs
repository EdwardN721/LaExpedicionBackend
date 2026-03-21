using LaExpedicion.Domain.Entities;

namespace LaExpedicion.Domain.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IGenericRepository<Personaje> Personajes { get; }
    IGenericRepository<Etiqueta> Etiquetas { get; }
    IGenericRepository<Item> Items { get; }
    IGenericRepository<ItemModificador> ItemModificadores { get; }
    IGenericRepository<Estadistica> Estadisticas { get; }
    IGenericRepository<Inventario> Inventarios { get; }
    IGenericRepository<Expedicion> Expediciones { get; }
    IGenericRepository<ExpedicionRealizada> ExpedicionRealizadas { get; }
    
    Task<int> SaveChangesAsync();
    // Gestión de Transacciones especificas
    Task BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
}