using Maggsoft.Core.IoC;
using Trimango.Data.Mssql.Entities;

namespace Trimango.Mssql.Services.Interfaces
{
    /// <summary>
    /// Çoklu dil desteği servisi interface'i
    /// </summary>
    public interface ILocalizationService : IService
    {
        /// <summary>
        /// Belirtilen anahtar için çeviri metnini döndürür
        /// </summary>
        Task<string> GetResourceAsync(string resourceName, Guid? languageId = null);

        /// <summary>
        /// Belirtilen anahtar için çeviri metnini döndürür (senkron)
        /// </summary>
        string GetResource(string resourceName, Guid? languageId = null);

        /// <summary>
        /// Belirtilen anahtar için çeviri metnini parametrelerle birlikte döndürür
        /// </summary>
        Task<string> GetResourceAsync(string resourceName, object[] parameters, Guid? languageId = null);

        /// <summary>
        /// Belirtilen anahtar için çeviri metnini parametrelerle birlikte döndürür (senkron)
        /// </summary>
        string GetResource(string resourceName, object[] parameters, Guid? languageId = null);

        /// <summary>
        /// Tüm aktif dilleri döndürür
        /// </summary>
        Task<List<Language>> GetActiveLanguagesAsync();

        /// <summary>
        /// Hata mesajını çevirir ve döndürür
        /// </summary>
        Task<string> GetLocalizedErrorMessageAsync(string errorKey, object[]? parameters = null, Guid? languageId = null);

        /// <summary>
        /// Hata mesajını çevirir ve döndürür (senkron)
        /// </summary>
        string GetLocalizedErrorMessage(string errorKey, object[]? parameters = null, Guid? languageId = null);

        /// <summary>
        /// Çeviri cache'ini temizler
        /// </summary>
        Task ClearLocalizationCacheAsync();

        /// <summary>
        /// Tüm localization verilerini cache'e yükler
        /// </summary>
        Task InitializeLocalizationCacheAsync();
    }
}
