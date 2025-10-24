using Trimango.Data.Mssql.Entities;
using Maggsoft.Mssql.Repository;
using Maggsoft.Cache;
using Microsoft.EntityFrameworkCore;
using Trimango.Mssql.Services.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Trimango.Mssql.Services.Services
{
    /// <summary>
    /// Çoklu dil desteği servisi
    /// </summary>
    public class LocalizationService : CustomBaseService, ILocalizationService
    {
        private readonly ICache _cache;
        private const string CacheKeyPrefix = "Localization_";
        private readonly IMssqlRepository<Language> _languageRepository;
        private readonly IMssqlRepository<LocaleStringResource> _localeStringResourceRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LocalizationService(
            IMssqlRepository<LocaleStringResource> localeStringResourceRepository,
            IMssqlRepository<Language> languageRepository,
            ICache cache,
            IHttpContextAccessor httpContextAccessor)
        {
            _localeStringResourceRepository = localeStringResourceRepository;
            _languageRepository = languageRepository;
            _cache = cache;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Belirtilen anahtar için çeviri metnini döndürür
        /// </summary>
        public async Task<string> GetResourceAsync(string resourceName, Guid? languageId = null)
        {
            if (string.IsNullOrEmpty(resourceName))
                return resourceName;

            var currentLanguageId = languageId ?? await GetLanguageIdFromHeaderAsync();

            return await _cache.GetAsync($"{CacheKeyPrefix}{currentLanguageId}_{resourceName}", TimeSpan.FromDays(1), async () =>
            {
                var resource = await _localeStringResourceRepository.FindAsync(r =>
                    r.ResourceName == resourceName &&
                    r.LanguageId == currentLanguageId &&
                    r.IsActive);

                return resource?.ResourceValue ?? resourceName;
            });
        }

        /// <summary>
        /// Belirtilen anahtar için çeviri metnini döndürür (senkron)
        /// </summary>
        public string GetResource(string resourceName, Guid? languageId = null)
        {
            if (string.IsNullOrEmpty(resourceName))
                return resourceName;

            var currentLanguageId = languageId ?? GetLanguageIdFromHeader();
            var cacheKey = $"{CacheKeyPrefix}{currentLanguageId}_{resourceName}";

            return _cache.Get(cacheKey, TimeSpan.FromDays(1), () =>
            {
                var resource = _localeStringResourceRepository.Find(r =>
                    r.ResourceName == resourceName &&
                    r.LanguageId == currentLanguageId &&
                    r.IsActive);

                return resource?.ResourceValue ?? resourceName;
            });
        }

        /// <summary>
        /// Belirtilen anahtar için çeviri metnini parametrelerle birlikte döndürür
        /// </summary>
        public async Task<string> GetResourceAsync(string resourceName, object[] parameters, Guid? languageId = null)
        {
            var resourceValue = await GetResourceAsync(resourceName, languageId);
            return string.Format(resourceValue, parameters);
        }

        /// <summary>
        /// Belirtilen anahtar için çeviri metnini parametrelerle birlikte döndürür (senkron)
        /// </summary>
        public string GetResource(string resourceName, object[] parameters, Guid? languageId = null)
        {
            var resourceValue = GetResource(resourceName, languageId);
            return string.Format(resourceValue, parameters);
        }

        /// <summary>
        /// Tüm aktif dilleri döndürür
        /// </summary>
        public async Task<List<Language>> GetActiveLanguagesAsync()
        {
            return await _cache.GetAsync("ActiveLanguages", TimeSpan.FromHours(1), async () =>
            {
                return await _languageRepository.Table
                    .Where(l => l.IsActive)
                    .OrderBy(l => l.DisplayOrder)
                    .ToListAsync();
            });
        }

        /// <summary>
        /// Hata mesajını çevirir ve döndürür
        /// </summary>
        public async Task<string> GetLocalizedErrorMessageAsync(string errorKey, object[]? parameters = null, Guid? languageId = null)
        {
            try
            {
                if (parameters != null && parameters.Length > 0)
                {
                    return await GetResourceAsync(errorKey, parameters, languageId);
                }
                else
                {
                    return await GetResourceAsync(errorKey, languageId);
                }
            }
            catch (Exception)
            {
                // Hata durumunda varsayılan mesajı döndür
                return errorKey;
            }
        }

        /// <summary>
        /// Hata mesajını çevirir ve döndürür (senkron)
        /// </summary>
        public string GetLocalizedErrorMessage(string errorKey, object[]? parameters = null, Guid? languageId = null)
        {
            try
            {
                if (parameters != null && parameters.Length > 0)
                {
                    return GetResource(errorKey, parameters, languageId);
                }
                else
                {
                    return GetResource(errorKey, languageId);
                }
            }
            catch (Exception)
            {
                // Hata durumunda varsayılan mesajı döndür
                return errorKey;
            }
        }

        /// <summary>
        /// Çeviri cache'ini temizler
        /// </summary>
        public async Task ClearLocalizationCacheAsync()
        {
            try
            {
                // Cache'den tüm localization anahtarlarını temizle
                if (_cache != null)
                {
                    await _cache.RemoveByPatternAsync(CacheKeyPrefix);
                    await _cache.RemoveAsync("AllLanguages");
                    await _cache.RemoveAsync("LocalizationCache");
                    await _cache.RemoveAsync("LanguageMapping");
                    await _cache.RemoveAsync("ActiveLanguages");
                }
            }
            catch (Exception)
            {
                // Cache temizleme hatası göz ardı edilir
                // Log edilebilir ama işlem devam etmeli
            }
        }

        /// <summary>
        /// Tüm localization verilerini cache'e yükler
        /// </summary>
        public async Task InitializeLocalizationCacheAsync()
        {
            try
            {
                var languages = await _languageRepository.Table
                    .Where(l => l.IsActive && !l.IsDeleted)
                    .OrderBy(l => l.DisplayOrder)
                    .ToListAsync();

                await _cache.GetAsync("AllLanguages", TimeSpan.FromDays(1), async () => languages);

                var resources = await _localeStringResourceRepository.Table
                    .Where(r => r.IsActive && !r.IsDeleted)
                    .ToListAsync();

                var localizationCache = resources
                    .GroupBy(r => $"{r.LanguageId}_{r.ResourceName}")
                    .ToDictionary(
                        g => g.Key, 
                        g => g.OrderByDescending(r => r.CreatedDate).First().ResourceValue
                    );
                await _cache.GetAsync("LocalizationCache", TimeSpan.FromDays(1), async () => localizationCache);

                var languageMapping = languages
                    .GroupBy(l => l.UniqueSeoCode)
                    .ToDictionary(
                        g => g.Key, 
                        g => g.OrderByDescending(l => l.CreatedDate).First().Id
                    );
                await _cache.GetAsync("LanguageMapping", TimeSpan.FromDays(1), async () => languageMapping);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Localization cache initialization failed", ex);
            }
        }
    }
}
