namespace Trimango.Api.Models;

/// <summary>
/// JWT ayarları için model sınıfı
/// </summary>
public class JwtSettings
{
    /// <summary>
    /// JWT gizli anahtarı
    /// </summary>
    public string SecretKey { get; set; } = string.Empty;
    
    /// <summary>
    /// JWT yayıncısı
    /// </summary>
    public string Issuer { get; set; } = string.Empty;
    
    /// <summary>
    /// JWT hedef kitlesi
    /// </summary>
    public string Audience { get; set; } = string.Empty;
    
    /// <summary>
    /// Token geçerlilik süresi (dakika)
    /// </summary>
    public int ExpirationInMinutes { get; set; }
    
    /// <summary>
    /// Refresh token geçerlilik süresi (gün)
    /// </summary>
    public int RefreshTokenExpirationInDays { get; set; }
} 