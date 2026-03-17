using System.Net;
using System.Text.Json;
using LaExpedicion.Domain.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace LaExpedicion.API.Middleware;

public class ExceptionMiddleware
{
    // RequestDelegate es un puntero que indica "El siguiente paso a ejecutar". 
    // Es la instrucción para decirle a la petición: "Pásale a lo que sigue".
    private readonly RequestDelegate _next;

    // Inyectamos el Logger para que,en nuestra consola de backend
    // quede registrado el error real.
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    // El metodo InvokeAsync es invocado automicaticamente por .NET 
    // con cada petición que llega a la API
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            // Intentamos ejecutar el resto del programa (Controladores, Servicios, Base de datos...)
            await _next(context);
        }
        catch (Exception ex)
        {
            // Si cualquier línea de código en TODO el proyecto hace un "throw", caerá aquí irremediablemente.
            _logger.LogError(ex, "Ocurrió un error en la aplicación.");

            // Si atrapamos un error, llamamos a nuestro método privado para crear el JSON de respuesta
            await HandleExceptionAsync(context, ex);
        }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        // 1. Configuramos que la respuesta que vamos a enviar será en formato JSON
        context.Response.ContentType = "application/json";

        int statusCode = (int)HttpStatusCode.InternalServerError;
        string message = "Ocurrió un error interno en el servidor.";

        // 2. Evaluamos qué tipo de error fue usando un switch
        switch (exception)
        {
            case NotFoundException:
            case KeyNotFoundException:
                statusCode = (int)HttpStatusCode.NotFound; // HTTP 404
                message = exception.Message;
                break;

            // Cuando le pasas un valor nulo o inválido a un método
            case ArgumentNullException:
            case ArgumentException:
                statusCode = (int)HttpStatusCode.BadRequest; // HTTP 400
                message = exception.Message;
                break;
            
            // ERRORES DE SEGURIDAD (Ej. Un usuario intentando entrar a un lugar sin permisos)
            case UnauthorizedAccessException:
                statusCode = (int)HttpStatusCode.Unauthorized; // HTTP 401
                message = "No tienes permisos suficientes para realizar esta acción.";
                break;

            // ERRORES DE BASE DE DATOS (Entity Framework Core)
            case DbUpdateException dbEx:
                statusCode = (int)HttpStatusCode.BadRequest;
                message = "Error al intentar guardar en la base de datos.";
                
                // Revisamos si el error interno viene directamente de SQL Server
                if (dbEx.InnerException is SqlException sqlEx)
                {
                    // SQL Server tiene códigos de error fijos. Evaluamos los más comunes:
                    switch (sqlEx.Number)
                    {
                        case 2601: // Duplicated key row
                        case 2627: // Unique constraint (Ej. Intentas registrar un correo que ya existe)
                            statusCode = (int)HttpStatusCode.Conflict; // HTTP 409
                            message = "Ya existe un registro con estos datos únicos (ej. correo o nombre de usuario duplicado).";
                            break;
                            
                        case 547: // Foreign Key / Constraint check 
                            statusCode = (int)HttpStatusCode.Conflict; // HTTP 409
                            message = "Operación denegada. Estás intentando usar un dato que no existe o eliminar un registro que está siendo utilizado por otros.";
                            break;
                    }
                }
                break;
            
            default:
                // Le asignamos HTTP 500 (Internal Server Error)
                message = exception.Message; 
                break;
        }

        // 3. Le asignamos el código final a la respuesta HTTP
        context.Response.StatusCode = statusCode;

        // 4. Armamos el JSON que recibirá el Frontend
        var response = new
        {
            StatusCode = statusCode,
            Message = message
        };

        return context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
}