using LaExpedicion.API.Extensions;
using LaExpedicion.API.Middleware;

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

// Swagger
builder.Services.AddSwaggerExtension();

// Activa el motor de Autorización en la memoria RAM
builder.Services.AddAuthorization();

// Fluent validator
builder.Services.AddValidationService();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger(); // <-- Genera el JSON de Swagger
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "LaExpedicion API v1")); // <-- Levanta la interfaz gráfica
}

app.UseCors("AllowFrontend");

app.UseMiddleware<ExceptionMiddleware>();

app.MapControllers(); // <-- enrute el tráfico hacia los controladores
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.Run();
