using System.Linq.Expressions;
using LaExpedicion.Domain.Entities;
using LaExpedicion.Domain.Interfaces;
using LaExpedicion.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LaExpedicion.Infrastructure.Repository;

public class GenericRepository<T> : IGenericRepository<T> where T 
    : BaseEntity 
{
    private readonly AppDbContext _context;
    private readonly DbSet<T> _dbSet;

    public GenericRepository(AppDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<T>();
    }

    public async Task<T?> ObtenerPorIdAsync(Guid id)
    {
        return await _dbSet.FindAsync(id); 
    }

    public async Task<IEnumerable<T>> ObtenerTodosAsync()
    {
        return await _dbSet.AsNoTracking().ToListAsync();
    }

    public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
    {
        return await _dbSet.AsNoTracking().Where(predicate).ToListAsync(); 
    }

    public async Task<(IEnumerable<T> Registros, int Total)> ObtenerPaginadosAsync(Expression<Func<T, bool>>? filtro, int pageNumber, int pageSize)
    {
        IQueryable<T> query = _dbSet.AsNoTracking();

        if (filtro != null)
        {
            query = query.Where(filtro);
        }
        
        int total = await query.CountAsync();
        
        var registros = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        
        return (registros, total);
    }

    public async Task<T?> GetFirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
    {
        return await _dbSet.FirstOrDefaultAsync(predicate); 
    }

    public async Task AgregarAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
    }

    public void Actualizar(T entity)
    {
        _dbSet.Attach(entity);
        _context.Entry(entity).State = EntityState.Modified;
    }

    public void Eliminar(T entity)
    {
        if (_dbSet.Entry(entity).State == EntityState.Detached)
        {
            _dbSet.Attach(entity);
        }
        _dbSet.Remove(entity);
    }
}