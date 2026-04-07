using System.Linq.Expressions;
using LaExpedicion.Application.DTOs.Peticion;
using LaExpedicion.Application.DTOs.Respuesta;
using LaExpedicion.Application.Interfaces;
using LaExpedicion.Application.Mappers;
using LaExpedicion.Application.Parameters;
using LaExpedicion.Domain.Entities;
using LaExpedicion.Domain.Enum;
using LaExpedicion.Domain.Exceptions;
using LaExpedicion.Domain.Interfaces;
using LaExpedicion.Shared.Pagination;
using Microsoft.Extensions.Logging;

namespace LaExpedicion.Application.Services;

public class InventarioService : IInventarioService
{
    private readonly IUnitOfWork _unitOfWork;
    private ILogger<InventarioService> _logger;

    public InventarioService(IUnitOfWork unitOfWork, ILogger<InventarioService> logger)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<PagedList<InventarioDto>> ObtenerInventarioDelPersonaje(Guid personajeId,
        InventarioParameters parametros)
    {
        // 1. Filtramos por el personaje obligatorio
        Expression<Func<Inventario, bool>> filter = x => x.PersonajeId == personajeId;

        if (!string.IsNullOrWhiteSpace(parametros.Nombre))
        {
            filter = x => x.PersonajeId == personajeId && x.Item!.Nombre.Contains(parametros.Nombre);
        }

        // 2. Le mandamos las tablas que queremos incluir
        var (registros, total) = await _unitOfWork.Inventarios.ObtenerPaginadosAsync(
            filter,
            parametros.PageNumber,
            parametros.PageSize,
            x => x.Item!, // 👈 Entity Framework hará un INNER JOIN con Items
            x => x.Item!.ItemModificador, // Trar los modificadores
            x => x.Personaje! // 👈 Entity Framework hará un INNER JOIN con Personajes
        );

        List<InventarioDto> inventarios = registros.Select(inventario => inventario.MapToDto()).ToList();

        return new PagedList<InventarioDto>(
            inventarios,
            total,
            parametros.PageNumber,
            parametros.PageSize);
    }

    public async Task<InventarioDto> AgregarItem(CrearInventarioDto item)
    {
        _logger.LogInformation("Agregando item {ItemId} al personaje {PersonajeId}", item.ItemId, item.PersonajeId);
        Inventario nuevoRegistro = item.MapToEntity();

        await _unitOfWork.Inventarios.AgregarAsync(nuevoRegistro);
        await _unitOfWork.SaveChangesAsync();

        _logger.LogInformation("Item agregado al inventario exitosamente. Id de inventario: {Id}", nuevoRegistro.Id);
        return nuevoRegistro.MapToDto();
    }

    public async Task ActualizarInventario(Guid id, ActualizarInventarioDto item, Guid usuarioId)
    {
        Inventario inventario = await ObtenerPorId(id);

        ValidarUsuarioReal(inventario.PersonajeId, usuarioId);
        
        inventario.Equipado = item.Equipado;
        inventario.UsosRestantes = item.UsosRestantes;

        _unitOfWork.Inventarios.Actualizar(inventario);
        await _unitOfWork.SaveChangesAsync();

        _logger.LogInformation("Inventario {Id} actualizado. Equipado: {Equipado}, Usos: {Usos}", id, item.Equipado,
            item.UsosRestantes);
    }

    public async Task EliminarInventario(Guid id)
    {
        Inventario inventario = await ObtenerPorId(id);
        _unitOfWork.Inventarios.Eliminar(inventario);
        await _unitOfWork.SaveChangesAsync();

        _logger.LogInformation("El registro de inventario {Id} fue descartado/eliminado.", id);
    }

    public async Task UsarItem(Guid inventarioId, int usoAGastar = 1)
    {
        // Traemos todo en una sola consulta para poder curar al personaje
        var inventario = await _unitOfWork.Inventarios.GetFirstOrDefaultAsync(
            i => i.Id == inventarioId,
            i => i.Item!,
            i => i.Personaje!,
            i => i.Personaje!.Estadistica!
        ) ?? throw new NotFoundException("Objeto no encontrado en la mochila.");
        
        if (inventario.UsosRestantes < usoAGastar)
            throw new Exception("El objeto se ha quedado sin usos.");

        // Lógica de Curación
        int curacion = 30 * usoAGastar;
        inventario.Personaje!.SaludActual += curacion;

        // Evitamos overheal (curar más de la salud máxima)
        if (inventario.Personaje.SaludActual > inventario.Personaje.Estadistica!.Salud)
        {
            inventario.Personaje.SaludActual = inventario.Personaje.Estadistica.Salud;
        }

        // Desgaste del ítem consumible
        inventario.UsosRestantes -= usoAGastar;

        if (inventario.UsosRestantes <= 0)
        {
            _unitOfWork.Inventarios.Eliminar(inventario);
            _logger.LogInformation("El item {Id} se quedó sin usos (se gastó por completo) y fue removido.",
                inventarioId);
        }
        else
        {
            _unitOfWork.Inventarios.Actualizar(inventario);
            _logger.LogInformation("Se usó el item {Id}. Usos restantes: {Usos}", inventarioId,
                inventario.UsosRestantes);
        }

        _unitOfWork.Personajes.Actualizar(inventario.Personaje);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task EquiparItem(Guid inventarioId, Guid usuarioId)
    {
        Inventario inventario = await _unitOfWork.Inventarios.GetFirstOrDefaultAsync(
            i => i.Id == inventarioId,
            i => i.Item!
        ) ?? throw new NotFoundException("Item no encontrado en la mochila.");

        ValidarUsuarioReal(inventario.PersonajeId, usuarioId);
        
        if (inventario.Equipado)
        {
            inventario.Equipado = false;
            _unitOfWork.Inventarios.Actualizar(inventario);
            await _unitOfWork.SaveChangesAsync();
            return;
        }

        if (inventario.Item!.TipoItem == EnumTipoItems.Consumible)
        {
            throw new Exception("No puedes equiparte un objeto consumible. Usa el botón de 'Usar'.");
        }

        var (equipados, _) = await _unitOfWork.Inventarios.ObtenerPaginadosAsync(
            i => i.PersonajeId == inventario.PersonajeId && i.Equipado == true,
            1, 50,
            i => i.Item!
        );

        EnumTipoItems tipoNuevo = inventario.Item.TipoItem;

        foreach (var itemEquipado in equipados)
        {
            EnumTipoItems tipoEquipado = itemEquipado.Item!.TipoItem;

            if (tipoNuevo == tipoEquipado)
            {
                itemEquipado.Equipado = false;
                _unitOfWork.Inventarios.Actualizar(itemEquipado);
            }
            else if ((tipoNuevo == EnumTipoItems.ArmaUnaMano || tipoNuevo == EnumTipoItems.ArmaDosManos) && 
                     (tipoEquipado == EnumTipoItems.ArmaUnaMano || tipoEquipado == EnumTipoItems.ArmaDosManos))
            {
                itemEquipado.Equipado = false;
                _unitOfWork.Inventarios.Actualizar(itemEquipado);
            }
        }

        inventario.Equipado = true;
        _unitOfWork.Inventarios.Actualizar(inventario);

        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<InventarioDto> ComprarItem(CrearInventarioDto dto, Guid usuarioId)
    {
        await _unitOfWork.BeginTransactionAsync();
        try
        {
            var personaje = await _unitOfWork.Personajes.ObtenerPorIdAsync(dto.PersonajeId) 
                            ?? throw new NotFoundException("Personaje no encontrado.");
            
            ValidarUsuarioReal(personaje.UsuarioId, usuarioId);
            
            var item = await _unitOfWork.Items.ObtenerPorIdAsync(dto.ItemId)
                       ?? throw new NotFoundException("Objeto no encontrado.");

            if (personaje.Dinero < item.Precio)
            {
                throw new Exception($"No tienes suficiente oro. Necesitas {item.Precio} monedas.");
            }

            personaje.Dinero -= item.Precio;
            _unitOfWork.Personajes.Actualizar(personaje);

            var inventarioExistente = await _unitOfWork.Inventarios.GetFirstOrDefaultAsync(
                i => i.PersonajeId == dto.PersonajeId && i.ItemId == dto.ItemId,
                i => i.Item!,
                i => i.Personaje!
            );

            Inventario inventarioFinal;

            if (inventarioExistente != null)
            {
                inventarioExistente.UsosRestantes += 5; 
                _unitOfWork.Inventarios.Actualizar(inventarioExistente);
                inventarioFinal = inventarioExistente;
            }
            else
            {
                inventarioFinal = new Inventario
                {
                    PersonajeId = dto.PersonajeId,
                    ItemId = dto.ItemId,
                    Equipado = false,
                    UsosRestantes = 5 
                };
                await _unitOfWork.Inventarios.AgregarAsync(inventarioFinal);
            }

            await _unitOfWork.SaveChangesAsync();
            await _unitOfWork.CommitTransactionAsync();

            inventarioFinal.Item = item;
            inventarioFinal.Personaje = personaje;

            return inventarioFinal.MapToDto();
        }
        catch (Exception)
        {
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }
    
    public async Task VenderItem(Guid inventarioId, Guid usuarioId)
    {
        await _unitOfWork.BeginTransactionAsync();
        try
        {
            var inventario = await _unitOfWork.Inventarios.GetFirstOrDefaultAsync(
                i => i.Id == inventarioId, 
                i => i.Item!, 
                i => i.Personaje!
            ) ?? throw new NotFoundException("El objeto no existe en tu mochila.");

            ValidarUsuarioReal(inventario.PersonajeId, usuarioId);
            
            double valorDeVenta = inventario.Item!.Precio * 0.5;

            inventario.Personaje!.Dinero += valorDeVenta;

            // Eliminamos el objeto de su mochila
            _unitOfWork.Inventarios.Eliminar(inventario);
            _unitOfWork.Personajes.Actualizar(inventario.Personaje);

            await _unitOfWork.SaveChangesAsync();
            await _unitOfWork.CommitTransactionAsync();
            
            _logger.LogInformation("Personaje {Personaje} vendió {Item} por {Oro} oro.", 
                inventario.Personaje.NombreUsuario, inventario.Item.Nombre, valorDeVenta);
        }
        catch (Exception)
        {
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }
    
    #region MetodosPrivados

    private async Task<Inventario> ObtenerPorId(Guid id)
    {
        Inventario? inventario = await _unitOfWork.Inventarios.ObtenerPorIdAsync(id);

        if (inventario == null)
        {
            _logger.LogWarning("No existe ningun Inventario con el Id: {Id}", id);
            throw new NotFoundException($"No exíste el inventario con el Id: {id}");
        }

        return inventario;
    }

    private void ValidarUsuarioReal(Guid personajeId, Guid usuarioId)
    {
        if (personajeId != usuarioId)
        {
            throw new UnauthorizedAccessException("¡ALERTA DE SEGURIDAD! Estás intentando usar un personaje que no te pertenece.");
        }
    } 

    #endregion
}