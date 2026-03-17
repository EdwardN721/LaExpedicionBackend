using LaExpedicion.Application.DTOs.Peticion;
using LaExpedicion.Application.DTOs.Respuesta;
using LaExpedicion.Application.Parameters;
using LaExpedicion.Shared.Pagination;

namespace LaExpedicion.Application.Interfaces;

public interface IEtiquetaService
{
    Task<PagedList<EtiquetaDto>> ObtenerEtiquetas(ItemParameters itemParameters);
    Task<EtiquetaDto> ObtenerEtiquetaPorId(Guid id);
    Task<Guid> ObtenerEtiquetaPorNombre(string nombre);
    Task<EtiquetaDto> CrearEtiqueta(CrearEtiquetaDto dto);
    Task ActualizarEtiqueta(Guid id, ActualizarEtiquetaDto dto);
    Task EliminarEtiqueta(Guid id);
}