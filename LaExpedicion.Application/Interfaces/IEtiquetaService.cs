using LaExpedicion.Application.DTOs.Peticion;
using LaExpedicion.Application.DTOs.Respuesta;

namespace LaExpedicion.Application.Interfaces;

public interface IEtiquetaService
{
    Task<IEnumerable<EtiquetaDto>> ObtenerEtiquetas();
    Task<EtiquetaDto> ObtenerEtiquetaPorId(Guid id);
    Task<Guid> ObtenerEtiquetaPorNombre(string nombre);
    Task<EtiquetaDto> CrearEtiqueta(CrearEtiquetaDto dto);
    Task ActualizarEtiqueta(Guid id, ActualizarEtiquetaDto dto);
    Task EliminarEtiqueta(Guid id);
}