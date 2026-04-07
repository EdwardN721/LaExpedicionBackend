using LaExpedicion.Application.DTOs.Peticion;
using LaExpedicion.Application.DTOs.Respuesta;
using LaExpedicion.Application.Parameters;
using LaExpedicion.Shared.Pagination;

namespace LaExpedicion.Application.Interfaces;

public interface IInventarioService
{
    Task<PagedList<InventarioDto>> ObtenerInventarioDelPersonaje(Guid personajeId, InventarioParameters parametros);
    Task<InventarioDto> AgregarItem(CrearInventarioDto item);
    Task ActualizarInventario(Guid id, ActualizarInventarioDto item, Guid usuarioId);
    Task EliminarInventario(Guid id);
    Task UsarItem(Guid inventarioId, int usoAGastar = 1);
    Task EquiparItem(Guid inventarioId, Guid usuarioId);
    Task<InventarioDto> ComprarItem(CrearInventarioDto item, Guid usuarioId);
    Task VenderItem(Guid inventarioId, Guid usuarioId);
}