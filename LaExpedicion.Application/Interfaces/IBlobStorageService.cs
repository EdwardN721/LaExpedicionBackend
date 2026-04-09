using Microsoft.AspNetCore.Http;

namespace LaExpedicion.Application.Interfaces;

public interface IBlobStorageService
{
    Task<string> SubirArchivo(IFormFile? file, string nombreCarpeta, string nombreArchivo);
}