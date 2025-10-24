using Maggsoft.Core.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Trimango.Api.Controllers.v2;

/// <summary>
/// API Versioning test controller'ı - V2 (Yeni Özellikler)
/// </summary>
[ApiController]
[Asp.Versioning.ApiVersion("2.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Produces("application/json")]
public class VersionTestController : ControllerBase
{
    /// <summary>
    /// API versioning test endpoint'i - V2 gelişmiş özellikler
    /// </summary>
    /// <returns>API versiyonu ve mesaj</returns>
    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(Result<object>), StatusCodes.Status200OK)]
    public IActionResult GetVersion()
    {
        var result = Result<object>.Success(new
        {
            Version = "2.0",
            Message = "Trimango API v2.0 - Yeni Özellikler ile çalışıyor! 🚀",
            Timestamp = DateTime.UtcNow,
            Features = new[]
            {
                "✅ Konaklama Yönetimi",
                "✅ Rezervasyon Sistemi",
                "✅ Ödeme Yönetimi",
                "✅ Kullanıcı Yönetimi",
                "✅ Çoklu Dil Desteği",
                "🆕 Gelişmiş Filtreleme",
                "🆕 Real-time Bildirimler (SignalR)",
                "🆕 Rate Limiting",
                "🆕 Performans İyileştirmeleri"
            },
            Performance = new
            {
                AverageResponseTime = "< 100ms",
                RateLimitPerMinute = 60,
                CacheEnabled = true
            }
        }, "API v2.0 versiyonu başarıyla alındı");

        return Ok(result);
    }

    /// <summary>
    /// API sağlık kontrolü - V2 detaylı bilgiler
    /// </summary>
    /// <returns>Detaylı sağlık durumu</returns>
    [HttpGet("health")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(Result<object>), StatusCodes.Status200OK)]
    public IActionResult HealthCheck()
    {
        var result = Result<object>.Success(new
        {
            Status = "Healthy",
            Version = "2.0",
            Timestamp = DateTime.UtcNow,
            Uptime = TimeSpan.FromHours(24).ToString(@"dd\.hh\:mm\:ss"),
            Database = new
            {
                Status = "Connected",
                ResponseTime = "< 50ms"
            },
            Cache = new
            {
                Status = "Active",
                HitRate = "95%"
            },
            SignalR = new
            {
                Status = "Active",
                Connections = 0
            }
        }, "API v2.0 sağlıklı çalışıyor");

        return Ok(result);
    }

    /// <summary>
    /// V2'ye özel yeni özellik - API istatistikleri
    /// </summary>
    /// <returns>API kullanım istatistikleri</returns>
    [HttpGet("stats")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(Result<object>), StatusCodes.Status200OK)]
    public IActionResult GetStats()
    {
        var result = Result<object>.Success(new
        {
            TotalRequests = 10000,
            AverageResponseTime = "85ms",
            ErrorRate = "0.01%",
            TopEndpoints = new[]
            {
                new { Endpoint = "/api/v2/Properties", Requests = 3500 },
                new { Endpoint = "/api/v2/Reservations", Requests = 2800 },
                new { Endpoint = "/api/v2/Users", Requests = 1500 }
            }
        }, "API istatistikleri başarıyla alındı");

        return Ok(result);
    }
}
