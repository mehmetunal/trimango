using Maggsoft.Core.Base;
using Maggsoft.Core.Infrastructure;
using Maggsoft.Core.Model;
using Maggsoft.Mssql.Repository;
using Maggsoft.Mssql.Services;
using Microsoft.AspNetCore.Http;
using Trimango.Data.Mssql.Entities;
using Microsoft.EntityFrameworkCore;
using Trimango.Mssql.Services.Interfaces;

namespace Trimango.Mssql.Services.Services
{
    /// <summary>
    /// Özel Base Service - Localization desteği ile
    /// </summary>
    public class CustomBaseService : BaseService
    {
        private IHttpContextAccessor? _httpContextAccessor;
        private IMssqlRepository<Language>? _languageRepository;
        private const string LanguageHeaderName = "X-Language";

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
            _languageRepository = null;
        }

        private IHttpContextAccessor GetHttpContextAccessor()
        {
            if (_httpContextAccessor == null)
            {
                _httpContextAccessor = MaggsoftContext.Current.Resolve<IHttpContextAccessor>();
            }
            return _httpContextAccessor;
        }

        private IMssqlRepository<Language> GetLanguageRepository()
        {
            if (_languageRepository == null)
            {
                _languageRepository = MaggsoftContext.Current.Resolve<IMssqlRepository<Language>>();
            }
            return _languageRepository;
        }

        #region Localization Methods

        /// <summary>
        /// Localized error message alır (async)
        /// </summary>
        public async Task<string> GetLocalizedErrorMessageAsync(string errorKey, object[]? parameters = null)
        {
            return await MaggsoftContext.Current.Resolve<ILocalizationService>().GetLocalizedErrorMessageAsync(errorKey, parameters);
        }

        /// <summary>
        /// Localized error message alır (sync)
        /// </summary>
        public string GetLocalizedErrorMessage(string errorKey, object[]? parameters = null)
        {
            try
            {
                if (MaggsoftContext.Current != null)
                {
                    var localizationService = MaggsoftContext.Current.Resolve<ILocalizationService>();
                    if (localizationService != null)
                    {
                        return localizationService.GetLocalizedErrorMessage(errorKey, parameters);
                    }
                }
            }
            catch
            {
                // Hata durumunda fallback mesaj döndür
            }

            // Fallback: Basit hata mesajı döndür
            return parameters != null && parameters.Length > 0
                ? string.Format(errorKey, parameters)
                : errorKey;
        }

        #endregion

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
        public Result<T> Failure<T>(string errorKey, object[]? parameters = null) where T : new()
        {
            return Result<T>.Failure(GetLocalizedErrorMessage(errorKey, parameters));
        }

        #endregion

        #region Common Error Methods

        /// <summary>
        /// 404 Not Found hatası
        /// </summary>
        public Result<T> NotFound<T>(string errorKey = "Error.NotFound", object[]? parameters = null) where T : new()
        {
            return Failure<T>(errorKey, parameters);
        }

        /// <summary>
        /// 400 Bad Request hatası
        /// </summary>
        public Result<T> BadRequest<T>(string errorKey = "Error.BadRequest", object[]? parameters = null) where T : new()
        {
            return Failure<T>(errorKey, parameters);
        }

        /// <summary>
        /// 401 Unauthorized hatası
        /// </summary>
        public Result<T> Unauthorized<T>(string errorKey = "Error.Unauthorized", object[]? parameters = null) where T : new()
        {
            return Failure<T>(errorKey, parameters);
        }

        /// <summary>
        /// 403 Forbidden hatası
        /// </summary>
        public Result<T> Forbidden<T>(string errorKey = "Error.Forbidden", object[]? parameters = null) where T : new()
        {
            return Failure<T>(errorKey, parameters);
        }

        /// <summary>
        /// 409 Conflict hatası
        /// </summary>
        public Result<T> Conflict<T>(string errorKey = "Error.Conflict", object[]? parameters = null) where T : new()
        {
            return Failure<T>(errorKey, parameters);
        }

        /// <summary>
        /// 422 Validation Failed hatası
        /// </summary>
        public Result<T> ValidationFailed<T>(string errorKey = "Error.ValidationFailed", object[]? parameters = null) where T : new()
        {
            return Failure<T>(errorKey, parameters);
        }

        /// <summary>
        /// 500 Internal Server Error hatası
        /// </summary>
        public Result<T> InternalServerError<T>(string errorKey = "Error.InternalServerError", object[]? parameters = null) where T : new()
        {
            return Failure<T>(errorKey, parameters);
        }

        #endregion

        #region Business Logic Error Methods

        /// <summary>
        /// Unique constraint hatası
        /// </summary>
        public Result<T> UniqueConstraint<T>(string errorKey, object[]? parameters = null) where T : new()
        {
            return Conflict<T>(errorKey, parameters);
        }

        /// <summary>
        /// Authentication hatası
        /// </summary>
        public Result<T> AuthenticationFailed<T>(string errorKey = "Error.AuthenticationFailed", object[]? parameters = null) where T : new()
        {
            return Unauthorized<T>(errorKey, parameters);
        }

        /// <summary>
        /// Authorization hatası
        /// </summary>
        public Result<T> AuthorizationFailed<T>(string errorKey = "Error.AuthorizationFailed", object[]? parameters = null) where T : new()
        {
            return Forbidden<T>(errorKey, parameters);
        }

        /// <summary>
        /// Operation failed hatası
        /// </summary>
        public Result<T> OperationFailed<T>(string errorKey = "Error.OperationFailed", object[]? parameters = null) where T : new()
        {
            return InternalServerError<T>(errorKey, parameters);
        }

        #endregion

        #region Entity Specific Error Methods

        /// <summary>
        /// User not found hatası
        /// </summary>
        public Result<T> UserNotFound<T>() where T : new()
        {
            return NotFound<T>("Error.UserNotFound");
        }

        /// <summary>
        /// Property not found hatası
        /// </summary>
        public Result<T> PropertyNotFound<T>() where T : new()
        {
            return NotFound<T>("Error.PropertyNotFound");
        }

        /// <summary>
        /// Supplier not found hatası
        /// </summary>
        public Result<T> SupplierNotFound<T>() where T : new()
        {
            return NotFound<T>("Error.SupplierNotFound");
        }

        /// <summary>
        /// City not found hatası
        /// </summary>
        public Result<T> CityNotFound<T>() where T : new()
        {
            return NotFound<T>("Error.CityNotFound");
        }

        /// <summary>
        /// District not found hatası
        /// </summary>
        public Result<T> DistrictNotFound<T>() where T : new()
        {
            return NotFound<T>("Error.DistrictNotFound");
        }

        /// <summary>
        /// Language not found hatası
        /// </summary>
        public Result<T> LanguageNotFound<T>() where T : new()
        {
            return NotFound<T>("Error.LanguageNotFound");
        }

        /// <summary>
        /// Location not found hatası
        /// </summary>
        public Result<T> LocationNotFound<T>() where T : new()
        {
            return NotFound<T>("Error.LocationNotFound");
        }

        /// <summary>
        /// PropertyType not found hatası
        /// </summary>
        public Result<T> PropertyTypeNotFound<T>() where T : new()
        {
            return NotFound<T>("Error.PropertyTypeNotFound");
        }

        #endregion

        #region Language Helper Methods

        /// <summary>
        /// Mevcut dil ID'sini döndürür
        /// </summary>
        public async Task<Guid> GetLanguageIdFromHeaderAsync()
        {
            var httpContextAccessor = GetHttpContextAccessor();
            var languageRepository = GetLanguageRepository();

            // Header'dan dil kodunu al (örn: tr, en, de)
            var languageUniqueSeoCodeHeader = httpContextAccessor.HttpContext?.Request.Headers[LanguageHeaderName].FirstOrDefault();

            if (!string.IsNullOrEmpty(languageUniqueSeoCodeHeader))
            {
                // UniqueSeoCode ile dil bul
                var language = await languageRepository.FindAsync(l =>
                    l.UniqueSeoCode == languageUniqueSeoCodeHeader && l.IsActive);

                if (language != null)
                    return language.Id;
            }

            // Varsayılan dil (Türkçe - tr)
            var defaultLanguage = await languageRepository.FindAsync(l => l.UniqueSeoCode == "tr" && l.IsActive);

            if (defaultLanguage == null)
            {
                // İlk aktif dili al
                defaultLanguage = await languageRepository.Table
                    .Where(l => l.IsActive)
                    .OrderBy(l => l.DisplayOrder)
                    .FirstOrDefaultAsync();
            }

            return defaultLanguage?.Id ?? Guid.Empty;
        }

        /// <summary>
        /// Header'dan dil ID'sini alır (senkron)
        /// </summary>
        public Guid GetLanguageIdFromHeader()
        {
            var httpContextAccessor = GetHttpContextAccessor();
            var languageRepository = GetLanguageRepository();

            // Header'dan dil kodunu al (örn: tr, en, de)
            var languageCodeHeader = httpContextAccessor.HttpContext?.Request.Headers[LanguageHeaderName].FirstOrDefault();

            if (!string.IsNullOrEmpty(languageCodeHeader))
            {
                // UniqueSeoCode ile dil bul
                var language = languageRepository.Find(l =>
                    l.UniqueSeoCode == languageCodeHeader && l.IsActive);

                if (language != null)
                    return language.Id;
            }

            // Varsayılan dil (Türkçe - tr)
            var defaultLanguage = languageRepository.Find(l => l.UniqueSeoCode == "tr" && l.IsActive);

            if (defaultLanguage == null)
            {
                // İlk aktif dili al
                defaultLanguage = languageRepository.Table
                    .Where(l => l.IsActive)
                    .OrderBy(l => l.DisplayOrder)
                    .FirstOrDefault();
            }

            return defaultLanguage?.Id ?? Guid.Empty;
        }

        #endregion
    }
}