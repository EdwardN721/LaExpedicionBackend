using LaExpedicion.API.Extensions;
using LaExpedicion.API.Middleware;
using Microsoft.AspNetCore.Identity;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddControllers(); 
builder.Services.AddEndpointsApiExplorer();

// DB
builder.Services.AddAppDbContext(builder.Configuration);

// Identity 
builder.Services.AddIdentityDb();

// Services
builder.Services.AddServices();

// Cors
builder.Services.AddCorsExtension(builder.Configuration);

// OpenApi
builder.Services.AddNativeOpenApi();
builder.Services.AddJwtAuthentication(builder.Configuration);

// Activa el motor de Autorización en la memoria RAM
builder.Services.AddAuthorization();

// Fluent validator
builder.Services.AddValidationService();

// Permite leer el Token en las capas inferiores
builder.Services.AddReadToken();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // Genera el archivo openapi.json de forma nativa en /openapi/v1.json
    app.MapOpenApi();
    
    // Mapea la nueva interfaz gráfica (Scalar) en la ruta /scalar/v1
    app.MapScalarApiReference(options =>
    {
        options.WithTitle("La Expedición RPG - API")
            .WithTheme(ScalarTheme.DeepSpace)
            .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
    });
}

app.UseCors("AllowFrontend");

app.UseMiddleware<ExceptionMiddleware>();

app.MapControllers(); // <-- enrute el tráfico hacia los controladores
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
    
    string[] rolesNuevos = { "Admin", "Player" };
    
    foreach (var rol in rolesNuevos)
    {
        if (!await roleManager.RoleExistsAsync(rol))
        {
            await roleManager.CreateAsync(new IdentityRole<Guid>(rol));
        }
    }
}

app.Run();
