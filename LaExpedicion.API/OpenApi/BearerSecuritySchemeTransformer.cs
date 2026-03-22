using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;

namespace LaExpedicion.API.OpenApi;

internal sealed class BearerSecuritySchemeTransformer : IOpenApiDocumentTransformer
{
    public Task TransformAsync(OpenApiDocument document, OpenApiDocumentTransformerContext context,
        CancellationToken cancellationToken)
    {
        var autenticationScheme = new OpenApiSecurityScheme
        {
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT",
            Description = "Pega aquí tu token JWT (sin la palabra Bearer)."
        };

        var requirement = new OpenApiSecurityRequirement
        {
            // 👈 CAMBIO 2: En la v3, la llave exige ser una "Referencia"
            [new OpenApiSecuritySchemeReference("Bearer")] = new List<string>()
        };

        document.Components ??= new OpenApiComponents();
        document.Components.SecuritySchemes ??= new Dictionary<string, IOpenApiSecurityScheme>();
        document.Components.SecuritySchemes!.Add("Bearer", autenticationScheme);


        foreach (var operation in document.Paths.Values.SelectMany(path => path.Operations!.Values))
        {
            // En V3, la lista de seguridad puede venir nula por defecto, así que la inicializamos si hace falta
            operation.Security ??= new List<OpenApiSecurityRequirement>();
            operation.Security.Add(requirement);
        }


        return Task.CompletedTask;
    }
}