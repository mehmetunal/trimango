using System.Security.Claims;
using Serilog.Context;

namespace Trimango.Api.Middleware;


/// <summary>
/// Serilog için UserId ve IPAddress enrichment middleware
/// </summary>
public class SerilogEnrichmentMiddleware
{
    private readonly RequestDelegate _next;

    public SerilogEnrichmentMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // UserId'yi LogContext'e ekle
        var userId = context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value 
                     ?? context.User?.FindFirst("sub")?.Value 
                     ?? "Anonymous";

        // IPAddress'i al (X-Forwarded-For header'ını da kontrol et)
        var ipAddress = context.Request.Headers["X-Forwarded-For"].FirstOrDefault()
                        ?? context.Request.Headers["X-Real-IP"].FirstOrDefault()
                        ?? context.Connection.RemoteIpAddress?.ToString()
                        ?? "Unknown";

        // IPv6 localhost'u IPv4'e çevir
        if (ipAddress == "::1" || ipAddress == "0.0.0.1")
        {
            ipAddress = "127.0.0.1";
        }

        // LogContext'e ekle
        using (LogContext.PushProperty("UserId", userId))
        using (LogContext.PushProperty("IPAddress", ipAddress))
        {
            await _next(context);
        }
    }
}

/// <summary>
/// Middleware extension metodu
/// </summary>
public static class SerilogEnrichmentMiddlewareExtensions
{
    public static IApplicationBuilder UseSerilogEnrichment(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<SerilogEnrichmentMiddleware>();
    }
}
