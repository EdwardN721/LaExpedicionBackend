using System.Reflection;
using LaExpedicion.Domain.Entities;
using LaExpedicion.Domain.Interfaces;
using LaExpedicion.Infrastructure.Data;
using LaExpedicion.Infrastructure.Interceptors;
using LaExpedicion.Infrastructure.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;

namespace LaExpedicion.API.Extensions;

public static class ApplicationServiceExtensions
{
    public static IServiceCollection AddAppDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<AuditInterceptors>();
        services.AddDbContext<AppDbContext>((sp, options) =>
        {
            string? connectionString = configuration.GetConnectionString("DefaultConnection");

            var interceptor = sp.GetRequiredService<AuditInterceptors>();

            options.UseSqlServer(connectionString)
                .AddInterceptors(interceptor);
        });

        return services;
    }

    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        
        return services;
    }

    public static IServiceCollection AddIdentityDb(this IServiceCollection services)
    {
        services.AddIdentity<Usuario, IdentityRole<Guid>>()
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();
        
        return services;
    }

    public static IServiceCollection AddCorsExtension(this IServiceCollection services, IConfiguration configuration)
    {
        string frontendUrl = configuration.GetValue<string>("FrontendUrl") ?? "http://localhost:4200";

        services.AddCors(options =>
        {
            options.AddPolicy("AllowFrontend",
                policy =>
                {
                    policy.WithOrigins(frontendUrl)
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
        });

        return services;
    }

    public static IServiceCollection AddSwaggerExtension(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "LaExpedicion API",
                Version = "v1",
                Description = "API que controla el aplicativo RPG de la expedicion",
                Contact = new OpenApiContact
                {
                    Name = "Eduardo",
                    Email = "soporte@mymail.com"
                },
                License = new OpenApiLicense
                {
                    Name = "LicenciaProv"
                }
            });

            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            if (File.Exists(xmlPath))
            {
                c.IncludeXmlComments(xmlPath);
            }

            // Opcional: soporte para múltiples versiones
            c.SwaggerDoc("v2", new OpenApiInfo
            {
                Title = "API de Ejemplo",
                Version = "v2",
                Description = "Segunda versión de la API con mejoras"
            });

            // JWT Añadir
        });
        
        return services;
    }
}