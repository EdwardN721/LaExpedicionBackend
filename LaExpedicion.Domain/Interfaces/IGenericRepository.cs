using System.Linq.Expressions;
using LaExpedicion.Domain.Entities;

namespace LaExpedicion.Domain.Interfaces;

public interface IGenericRepository<T> where T : BaseEntity
{
    Task<T?> ObtenerPorIdAsync(Guid id);
    Task<IEnumerable<T>> ObtenerTodosAsync();
    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
   Task<(IEnumerable<T> Registros, int Total)> ObtenerPaginadosAsync(Expression<Func<T, bool>>? filtro, int pageNumber, int pageSize); 
    Task<T?> GetFirstOrDefaultAsync(Expression<Func<T, bool>> predicate);
    Task AgregarAsync(T entity);
    void Actualizar(T entity);
    void Eliminar(T entity);
}