using Dsw2025Ej15.Application.Exceptions;
using System.Net;
using System.Text.Json;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context); // Sigue al siguiente middleware/controlador
        }
        catch (AppException ex)
        {
            // Excepción controlada (negocio)
            context.Response.StatusCode = ex.StatusCode;
            context.Response.ContentType = "application/json";

            var error = JsonSerializer.Serialize(new { error = ex.Message });
            await context.Response.WriteAsync(error);
        }
        catch (Exception ex)
        {
            // Excepción inesperada (programación, conexión, etc.)
            _logger.LogError(ex, "Unhandled exception");

            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";

            var error = JsonSerializer.Serialize(new { error = "Error interno del servidor." });
            await context.Response.WriteAsync(error);
        }
    }
}
