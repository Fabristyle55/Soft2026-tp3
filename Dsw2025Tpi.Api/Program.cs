using Dsw2025Tpi.Data;
using Dsw2025Tpi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Dsw2025Tpi.Data.helpers;
using Dsw2025Ej15.Application.Services;
using Dsw2025Tpi.Data.Repositories;
using Dsw2025Tpi.Domain.Interfaces;
using Dsw2025Tpi.Application.Services;


namespace Dsw2025Tpi.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // La cadena de conexión NO está en el código: se lee de la configuración.
        // - Local: appsettings.Development.json (LocalDB).
        // - Docker / Azure: variable de entorno ConnectionStrings__Default
        //   (o la Connection String "Default" de tipo SQLAzure del App Service).
        var connectionString = builder.Configuration.GetConnectionString("Default")
            ?? throw new InvalidOperationException(
                "Falta la cadena de conexión 'Default'. Definí la variable de entorno ConnectionStrings__Default.");

        builder.Services.AddDbContext<Dsw2025TpiContext>(options =>
            options.UseSqlServer(connectionString, sql =>
                // Azure SQL (plan free) se auto-pausa: reintentamos mientras se reanuda.
                sql.EnableRetryOnFailure(maxRetryCount: 5, maxRetryDelay: TimeSpan.FromSeconds(10), errorNumbersToAdd: null)));

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(options =>
            options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
            {
                Title = "Dsw2025Tpi API",
                Version = "v1",
                Description = "API del e-commerce (UTN FRT - ICS 2026)"
            }));
        builder.Services.AddHealthChecks();

        // CORS para el frontend React: orígenes permitidos por configuración (Cors__AllowedOrigins__0, ...).
        var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? [];
        builder.Services.AddCors(options =>
            options.AddDefaultPolicy(policy =>
                policy.WithOrigins(allowedOrigins).AllowAnyHeader().AllowAnyMethod()));

        // Repositorio e inyección de dependencias
        builder.Services.AddScoped<IRepository, EfRepository>();

        // Servicios de aplicación
        builder.Services.AddScoped<ProductManagementService>();
        builder.Services.AddScoped<OrderManagementService>();

        var app = builder.Build();

        // Seeder
        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            try
            {
                var context = services.GetRequiredService<Dsw2025TpiContext>();
                context.Database.Migrate(); // Aplica las migraciones pendientes
                context.Seedwork<Customer>("Sources/customers.json"); // Usa el método de extensión para seedear los clientes
            }
            catch (Exception ex)
            {
                var logger = services.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "An error occurred creating the DB.");
            }
        }

        // Swagger habilitado también fuera de Development para poder probar la API
        // desde el dominio que asigna Azure (https://<app>.azurewebsites.net/swagger).
        // Se puede apagar con la variable de entorno Swagger__Enabled=false.
        if (app.Environment.IsDevelopment() || app.Configuration.GetValue("Swagger:Enabled", true))
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Dsw2025Tpi API v1");
                options.DocumentTitle = "Dsw2025Tpi API";
            });

            // La raíz del dominio redirige a Swagger.
            app.MapGet("/", () => Results.Redirect("/swagger")).ExcludeFromDescription();
        }

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            // En Docker y en Azure el TLS lo termina el proxy/plataforma: el contenedor escucha HTTP en 8080.
            app.UseHttpsRedirection();
        }

        app.UseMiddleware<ExceptionHandlingMiddleware>();

        app.UseCors();

        app.UseAuthorization();

        app.MapControllers();
        
        app.MapHealthChecks("/healthcheck");

        app.Run();
    }
}
