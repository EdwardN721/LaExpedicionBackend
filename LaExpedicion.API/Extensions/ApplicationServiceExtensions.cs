using System.Text;
using FluentValidation;
using LaExpedicion.API.OpenApi;
using LaExpedicion.Application.Interfaces;
using LaExpedicion.Application.Services;
using LaExpedicion.Application.Validations;
using LaExpedicion.Domain.Entities;
using LaExpedicion.Domain.Interfaces;
using LaExpedicion.Infrastructure.Data;
using LaExpedicion.Infrastructure.Interceptors;
using LaExpedicion.Infrastructure.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;

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
        services.AddScoped<ITokenService, TokenService>();

        services.AddScoped<IEstadisticaService, EstadisticaService>();
        services.AddScoped<IEtiquetaService, EtiquetaService>();
        services.AddScoped<IExpedicionService, ExpedicionService>();
        services.AddScoped<IInventarioService, InventarioService>();
        services.AddScoped<IItemService, ItemService>();
        services.AddScoped<IPersonajeService, PersonajeService>();
        services.AddScoped<IUsuarioService, UsuarioService>();
        services.AddScoped<IExpedicionRealizadaService, ExpedicionRealizadaService>();
        services.AddScoped<IEventoService, EventoService>();
        
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
                        .AllowAnyMethod()
                        .WithExposedHeaders("X-Pagination");
                });
        });

        return services;
    }

    public static IServiceCollection AddNativeOpenApi(this IServiceCollection services)
    {
        services.AddOpenApi(options =>
        {
            options.AddDocumentTransformer<BearerSecuritySchemeTransformer>();
        });
        
        return services;
    }

    public static IServiceCollection AddJwtAuthentication(this IServiceCollection services,
        IConfiguration configuration)
    {
        // 👇 AQUÍ ESTÁ LA MAGIA: Obligamos a usar JWT como esquema por defecto en toda la app
        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!)),
                    ValidateIssuer = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = configuration["Jwt:Audience"],
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
            });
        
        return services;
    }

    public static IServiceCollection AddValidationService(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<UsuarioValidator>();

        services.AddFluentValidationAutoValidation();
        
        return services;
    }
}