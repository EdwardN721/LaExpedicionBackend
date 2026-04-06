using System.Text.Json;
using LaExpedicion.Application.DTOs.Respuesta;
using LaExpedicion.Application.Interfaces;

namespace LaExpedicion.Application.Services;

public class EventoService : IEventoService
{
    public async Task<EventoExpedicionDto> ObtenerEventoAleatorioAsync()
    {
        string rutaArchivo = Path.Combine(Directory.GetCurrentDirectory(), "Data", "EventosExpedicion.json");
        
        if (!File.Exists(rutaArchivo))
            throw new FileNotFoundException("El archivo de eventos no existe. Verifica la ruta: " + rutaArchivo);
        
        // Leemos el texto completo
        string jsonString = await File.ReadAllTextAsync(rutaArchivo);

        JsonSerializerOptions opcionesJson = new JsonSerializerOptions { PropertyNameCaseInsensitive =  true };
        List<EventoExpedicionDto>? eventos =
            JsonSerializer.Deserialize<List<EventoExpedicionDto>>(jsonString, opcionesJson);
        
        if (eventos == null || !eventos.Any())
            throw new Exception("El archivo de eventos está vacío o mal formateado.");
        
        int indiceAleatorio = Random.Shared.Next(eventos.Count);
        return eventos[indiceAleatorio];
    }
}