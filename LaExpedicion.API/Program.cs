using LaExpedicion.API.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger(); // <-- Genera el JSON de Swagger
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "LaExpedicion API v1")); // <-- Levanta la interfaz gráfica
}

app.UseCors("AllowFrontend");

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.Run();
