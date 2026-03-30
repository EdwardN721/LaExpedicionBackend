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
            // 1. Validar que el personaje y la expedición existen (Y traemos sus estadísticas)
            Personaje personaje = await _unitOfWork.Personajes.GetFirstOrDefaultAsync(
                p => p.Id == personajeId,
                p => p.Estadistica!
            ) ?? throw new NotFoundException("El personaje no existe.");

            Expedicion expedicion = await _unitOfWork.Expediciones.ObtenerPorIdAsync(expedicionId)
                                    ?? throw new NotFoundException("La expedición no existe.");

            // 2. Prevención de Muerte
            if (personaje.SaludActual <= 5)
            {
                throw new Exception(
                    "Estás demasiado herido para explorar. Usa pociones desde tu mochila para curarte.");
            }

            // 3. Traer los ítems EQUIPADOS del inventario (con sus modificadores)
            var (registrosInventario, _) = await _unitOfWork.Inventarios.ObtenerPaginadosAsync(
                i => i.PersonajeId == personajeId && i.Equipado == true,
                1, 50,
                i => i.Item!,
                i => i.Item!.ItemModificador
            );
            var inventarioEquipado = registrosInventario.ToList();

            // 4. LÓGICA DE BATALLA: Base + Nivel + Modificadores
            int poderTotal = personaje.Estadistica!.Fuerza + personaje.Estadistica.Magia +
                             personaje.Estadistica.Energia + personaje.Estadistica.Mana;

            poderTotal += (personaje.Nivel * 5); // Bono de nivel

            // Sumamos los buffs de los items equipados
            foreach (var inv in inventarioEquipado)
            {
                foreach (var mod in inv.Item!.ItemModificador)
                {
                    // Aquí suma todo el poder extra que te den tus armas/armaduras
                    poderTotal += mod.ValorAjuste;
                }
            }

            // Dificultad Dinámica vs Suerte
            int dificultad = (expedicion.Experiencia + (int)expedicion.Dinero) / 2;
            int tiradaDeDados = Random.Shared.Next(1, 101); // Número del 1 al 100
            int puntuacionFinal = tiradaDeDados + poderTotal;

            bool esExito = puntuacionFinal >= dificultad;

            int expGanada = 0;
            int dineroGanado = 0;

            // 5. Resultados del Combate y Level Up
            if (esExito)
            {
                expGanada = expedicion.Experiencia;
                dineroGanado = (int)expedicion.Dinero;

                personaje.Dinero += dineroGanado;
                personaje.Experiencia += expGanada;

                int experienciaNecesaria = personaje.Nivel * 100;
                if (personaje.Experiencia >= experienciaNecesaria)
                {
                    personaje.Nivel++;
                    personaje.Experiencia -= experienciaNecesaria;

                    personaje.Estadistica.Salud += 20;
                    personaje.SaludActual = personaje.Estadistica.Salud;
                    personaje.Estadistica.Fuerza += 3;
                    personaje.Estadistica.Magia += 3;
                    personaje.Estadistica.Energia += 3;
                    personaje.Estadistica.Mana += 3;

                    _logger.LogInformation("¡El personaje {Personaje} ha subido al nivel {Nivel}!",
                        personaje.NombreUsuario, personaje.Nivel);
                }
            }
            else
            {
                // Castigo por perder
                int danoRecibido = Random.Shared.Next(15, 35);
                personaje.SaludActual -= danoRecibido;
                if (personaje.SaludActual < 1) personaje.SaludActual = 1;
            }

            // 6. Desgastar los objetos usados (Durabilidad)
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

            // 7. Preparar el registro de la expedición
            var registro = new ExpedicionRealizada
            {
                PersonajeId = personajeId,
                ExpedicionId = expedicionId,
                FechaInicio = DateTime.UtcNow,
                FechaFin = DateTime.UtcNow,
                Resultado = esExito ? EnumResultado.Exito : EnumResultado.Fracaso,
                ExperienciaGanada = expGanada,
                DineroGanado = dineroGanado
            };

            // 8. Guardar todo atómicamente
            await _unitOfWork.ExpedicionRealizadas.AgregarAsync(registro);
            _unitOfWork.Personajes.Actualizar(personaje);
            _unitOfWork.Estadisticas.Actualizar(personaje.Estadistica);

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