using LaExpedicion.Domain.Entities;
using LaExpedicion.Domain.Interfaces;
using LaExpedicion.Infrastructure.Data;
using Microsoft.EntityFrameworkCore.Storage;

namespace LaExpedicion.Infrastructure.Repository;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    private IDbContextTransaction? _transaction;

    private IGenericRepository<Personaje>? _personajes;
    private IGenericRepository<Item>? _items;
    private IGenericRepository<ItemModificador>? _itemModificadores;
    private IGenericRepository<Estadistica>? _estadisticas;
    private IGenericRepository<Inventario>? _inventarios;
    private IGenericRepository<Expedicion>? _expediciones;
    private IGenericRepository<ExpedicionRealizada>? _expedicionRealizadas;
    private IGenericRepository<Etiqueta>? _etiquetas;

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
    }

    public IGenericRepository<Personaje> Personajes
    {
        get
        {
            return _personajes ??= new GenericRepository<Personaje>(_context); 
        } 
    }

    public IGenericRepository<Estadistica> Estadisticas
    {
        get
        {
            return _estadisticas ??= new GenericRepository<Estadistica>(_context);
        }
    }

    public IGenericRepository<Etiqueta> Etiquetas
    {
        get
        {
            return _etiquetas ??= new GenericRepository<Etiqueta>(_context);
        }
    }

    public IGenericRepository<Item> Items
    {
        get
        {
            return _items ??= new GenericRepository<Item>(_context);
        }
    }

    public IGenericRepository<ItemModificador> ItemModificadores
    {
        get
        {
            return _itemModificadores ??= new GenericRepository<ItemModificador>(_context);
        }
    }

    public IGenericRepository<Inventario> Inventarios
    {
        get
        {
            return _inventarios ??= new GenericRepository<Inventario>(_context);
        }
    }

    public IGenericRepository<Expedicion> Expediciones
    {
        get
        {
            return _expediciones ??= new GenericRepository<Expedicion>(_context);
        }
    }

    public IGenericRepository<ExpedicionRealizada> ExpedicionRealizadas
    {
        get
        {
            return _expedicionRealizadas ??= new GenericRepository<ExpedicionRealizada>(_context);
        }
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public async Task BeginTransactionAsync()
    {
        if (_transaction != null)
        {
            return;
        }
        _transaction = await _context.Database.BeginTransactionAsync(); 
    }

    public async Task CommitTransactionAsync()
    {
        try
        {
            await _context.SaveChangesAsync();

            if (_transaction != null)
            {
                await _transaction.CommitAsync();
            }
        }
        catch
        {
            await RollbackTransactionAsync();
            throw;
        }
        finally
        {
            if (_transaction != null)
            {
                _transaction.Dispose();
                _transaction = null;
            }
        }
    }

    public async Task RollbackTransactionAsync()
    {
        try
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
            }
        }
        finally
        {
            if (_transaction != null)
            {
                _transaction.Dispose();
                _transaction = null;    
            }
        }
    }
    
    public void Dispose()
    {
        if (_transaction != null)
        {
            _transaction.Dispose();
        }
        _context.Dispose();
    }
}