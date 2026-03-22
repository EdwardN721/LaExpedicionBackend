using LaExpedicion.Domain.Enum;
using LaExpedicion.Domain.Entities;
using Microsoft.Extensions.Logging;
using LaExpedicion.Domain.Exceptions;
using LaExpedicion.Domain.Interfaces;
using LaExpedicion.Shared.Pagination;
using LaExpedicion.Application.Mappers;
using LaExpedicion.Application.Interfaces;
using LaExpedicion.Application.Parameters;
using LaExpedicion.Application.DTOs.Respuesta;

namespace LaExpedicion.Application.Services;

public class ExpedicionRealizadaService : IExpedicionRealizadaService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<ExpedicionRealizadaService> _logger;

    public ExpedicionRealizadaService(IUnitOfWork unitOfWork, ILogger<ExpedicionRealizadaService> logger)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }
    
    public async Task<PagedList<ExpedicionRealizadaDto>> ObtenerHistorialDePersonaje(Guid personajeId,
        RequestParameters parameters)
    {
        var (registros, total) = await _unitOfWork.ExpedicionRealizadas.ObtenerPaginadosAsync(
            x => x.PersonajeId == personajeId,
            parameters.PageNumber,
            parameters.PageSize,
            x => x.Expedicion! // Hacemos el JOIN para saber el nombre del lugar
        );

        List<ExpedicionRealizadaDto> dtos = registros.Select(r => r.MapToDto()).ToList();
        return new PagedList<ExpedicionRealizadaDto>(dtos, total, parameters.PageNumber, parameters.PageSize);
    }

    public async Task<ExpedicionRealizadaDto> EmprenderExpedicion(Guid personajeId, Guid expedicionId)
    {
        await _unitOfWork.BeginTransactionAsync();

        try
        {
            // 1. Validar que el personaje y la expedición existen
            Personaje personaje = await _unitOfWork.Personajes.ObtenerPorIdAsync(personajeId)
                                   ?? throw new NotFoundException("El personaje no existe.");

            Expedicion expedicion = await _unitOfWork.Expediciones.ObtenerPorIdAsync(expedicionId)
                                     ?? throw new NotFoundException("La expedicion no existe.");

            // 2. Traer las estadísticas base del personaje
            Estadistica estadisticas =
                await _unitOfWork.Estadisticas.GetFirstOrDefaultAsync(e => e.PersonajeId == personajeId)
                ?? throw new NotFoundException("El personaje no tiene estadísticas.");

            // 3. Traer los ítems EQUIPADOS del inventario (con sus modificadores)
            var (registrosInventario, _) = await _unitOfWork.Inventarios.ObtenerPaginadosAsync(
                i => i.PersonajeId == personajeId && i.Equipado == true,
                1, 50,
                i => i.Item!,
                i => i.Item!.ItemModificador
            );
            
            var inventarioEquipado = registrosInventario.ToList();

            // 4. LÓGICA DE BATALLA / RNG (Tirar los dados)
            // Aquí puedes hacer el cálculo tan complejo como quieras.
            // Por ahora, sumamos un dado (1 al 100) + la fuerza + la magia
            int poderTotal = estadisticas.Fuerza + estadisticas.Magia;

            // Sumamos los buffs de los items equipados
            foreach (var inv in inventarioEquipado)
            {
                foreach (var mod in inv.Item!.ItemModificador)
                {
                    if (mod.EstadisticaAfectada == EnumEstadistica.Fuerza ||
                        mod.EstadisticaAfectada == EnumEstadistica.Magia)
                    {
                        poderTotal += mod.ValorAjuste;
                    }
                }
            }

            int tiradaDeDados = Random.Shared.Next(1, 101); // Número del 1 al 100
            int puntuacionFinal = tiradaDeDados + poderTotal;

            // Para ganar cualquier expedición necesitas sacar más de 75 puntos
            bool esExito = puntuacionFinal >= 75;

            // 5. Desgastar los objetos usados (Durabilidad)
            foreach (var inv in inventarioEquipado)
            {
                inv.UsosRestantes -= 1;
                if (inv.UsosRestantes <= 0)
                {
                    _unitOfWork.Inventarios.Eliminar(inv);
                    _logger.LogInformation("El item {Item} se rompió durante la expedición.", inv.Item!.Nombre);
                }
                else
                {
                    _unitOfWork.Inventarios.Actualizar(inv);
                }
            }

            // 6. Preparar el registro de la expedición
            var registro = new ExpedicionRealizada
            {
                PersonajeId = personajeId,
                ExpedicionId = expedicionId,
                FechaInicio = DateTime.UtcNow,
                FechaFin = DateTime.UtcNow,
                Resultado = esExito ? EnumResultado.Exito : EnumResultado.Fracaso,
                ExperienciaGanada = esExito ? expedicion.Experiencia : 0,
                DineroGanado = esExito ? expedicion.Dinero : 0
            };

            // 7. Experiencia o Dinero
            // personaje.ExperienciaTotal += registro.ExperienciaGanada;
            // personaje.DineroTotal += registro.DineroGanado;
            // _unitOfWork.Personajes.Actualizar(personaje);

            await _unitOfWork.ExpedicionRealizadas.AgregarAsync(registro);

            // 8. Guardar todo (Registro, Desgaste de items, y Stats del personaje)
            await _unitOfWork.SaveChangesAsync();
            await _unitOfWork.CommitTransactionAsync();

            _logger.LogInformation("Expedición {Expedicion} finalizada por {Personaje}. Resultado: {Resultado}",
                expedicion.Nombre, personaje.NombreUsuario, registro.Resultado);

            return registro.MapToDto();
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackTransactionAsync();
            _logger.LogError(ex, "Error crítico durante la expedición.");
            throw;
        }
    }
}