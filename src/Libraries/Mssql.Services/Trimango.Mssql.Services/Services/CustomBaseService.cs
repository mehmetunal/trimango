using Maggsoft.Core.Base;
using Maggsoft.Core.Infrastructure;
using Maggsoft.Core.Model;
using Maggsoft.Mssql.Repository;
using Maggsoft.Mssql.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Trimango.Data.Mssql;

namespace Trimango.Mssql.Services.Services;

/// <summary>
/// Özel Base Service - Maggsoft Framework BaseService'ten türetilir
/// </summary>
public class CustomBaseService : BaseService
{
    private IHttpContextAccessor? _httpContextAccessor;

    /// <summary>
    /// <summary>
    /// Kullanıcının User-Agent bilgisi
    /// </summary>
    public string UserAgent
    {
        get
        {
            try
            {
                var httpContextAccessor = GetHttpContextAccessor();
                var userAgent = httpContextAccessor?.HttpContext?.Request.Headers["User-Agent"].FirstOrDefault();
                return !string.IsNullOrEmpty(userAgent) ? userAgent : "Unknown";
            }
            catch
            {
                return "Unknown";
            }
        }
    }

    /// <summary>
    /// Kullanıcının Admin rolüne sahip olup olmadığını kontrol eder
    /// BaseService.CurrentUserRole'den otomatik hesaplanır
    /// </summary>
    protected bool IsAdmin
    {
        get
        {
            try
            {
                return !string.IsNullOrEmpty(CurrentUserRole) && CurrentUserRole == "Admin";
            }
            catch
            {
                return false;
            }
        }
    }
    
    public CustomBaseService()
    {
        // Lazy initialization - resolve dependencies when needed
        _httpContextAccessor = null;
    }

    
    private IHttpContextAccessor GetHttpContextAccessor()
    {
        if (_httpContextAccessor == null)
        {
            _httpContextAccessor = MaggsoftContext.Current.Resolve<IHttpContextAccessor>();
        }
        return _httpContextAccessor;
    }

    

    #region Success Methods

    /// <summary>
    /// Başarılı sonuç döndürür
    /// </summary>
    public Result<T> Success<T>(T data) where T : new()
    {
        return Result<T>.Success(data);
    }

    #endregion

    #region Failure Methods

    /// <summary>
    /// Hata sonucu döndürür (500 Internal Server Error)
    /// </summary>
    public Result<T> Failure<T>(string errorKey) where T : new()
    {
        return Result<T>.Failure(errorKey);
    }

    /// <summary>
    /// İşlem başarısız sonucu döndürür
    /// </summary>
    public Result<T> OperationFailed<T>(string errorKey, object[]? parameters = null) where T : new()
    {
        var message = parameters != null && parameters.Length > 0 
            ? $"{errorKey}: {string.Join(", ", parameters)}" 
            : errorKey;
        return Result<T>.Failure(message);
    }

    #endregion

    #region Common Error Methods

    /// <summary>
    /// 404 Not Found hatası
    /// </summary>
    public Result<T> NotFound<T>(string errorKey = "Error.NotFound") where T : new()
    {
        return Failure<T>(errorKey);
    }

    /// <summary>
    /// 400 Bad Request hatası
    /// </summary>
    public Result<T> BadRequest<T>(string errorKey = "Error.BadRequest") where T : new()
    {
        return Failure<T>(errorKey);
    }

    /// <summary>
    /// 401 Unauthorized hatası
    /// </summary>
    public Result<T> Unauthorized<T>(string errorKey = "Error.Unauthorized") where T : new()
    {
        return Failure<T>(errorKey);
    }

    /// <summary>
    /// 403 Forbidden hatası
    /// </summary>
    public Result<T> Forbidden<T>(string errorKey = "Error.Forbidden") where T : new()
    {
        return Failure<T>(errorKey);
    }

    /// <summary>
    /// 409 Conflict hatası
    /// </summary>
    public Result<T> Conflict<T>(string errorKey = "Error.Conflict") where T : new()
    {
        return Failure<T>(errorKey);
    }

    #endregion

    #region Entity Specific Error Methods

    /// <summary>
    /// Tedarikçi bulunamadı hatası
    /// </summary>
    public Result<T> SupplierNotFound<T>() where T : new()
    {
        return NotFound<T>("Error.SupplierNotFound");
    }

    /// <summary>
    /// Emlak bulunamadı hatası
    /// </summary>
    public Result<T> PropertyNotFound<T>() where T : new()
    {
        return NotFound<T>("Error.PropertyNotFound");
    }

    /// <summary>
    /// Emlak türü bulunamadı hatası
    /// </summary>
    public Result<T> PropertyTypeNotFound<T>() where T : new()
    {
        return NotFound<T>("Error.PropertyTypeNotFound");
    }

    /// <summary>
    /// Lokasyon bulunamadı hatası
    /// </summary>
    public Result<T> LocationNotFound<T>() where T : new()
    {
        return NotFound<T>("Error.LocationNotFound");
    }

    #endregion
}