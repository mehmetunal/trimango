using Trimango.Data.Mssql;
using Trimango.Data.Mssql.Enums;

namespace Trimango.Data.Mssql.Entities
{
    /// <summary>
    /// Sistem desteklenen dilleri temsil eden entity
    /// Çoklu dil desteği için kullanılır
    /// </summary>
    public class Language : BaseEntity
    {
        /// <summary>
        /// Dil adı (Türkçe, English, Deutsch vb.)
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Dil kültür kodu (tr-TR, en-US, de-DE vb.)
        /// </summary>
        public string LanguageCulture { get; set; } = string.Empty;

        /// <summary>
        /// Benzersiz SEO kodu (tr, en, de vb.)
        /// </summary>
        public string UniqueSeoCode { get; set; } = string.Empty;

        /// <summary>
        /// Bayrak resmi dosya adı
        /// </summary>
        public string? FlagImageFileName { get; set; }

        /// <summary>
        /// Sağdan sola yazım desteği (Arapça, İbranice vb.)
        /// </summary>
        public bool Rtl { get; set; } = false;

        /// <summary>
        /// Dil durumu
        /// </summary>
        public LanguageState State { get; set; } = LanguageState.Active;

        // Navigation Properties
        /// <summary>
        /// Dile ait çeviri kaynakları
        /// </summary>
        public virtual ICollection<LocaleStringResource> LocaleStringResources { get; set; } = new List<LocaleStringResource>();

        /// <summary>
        /// Dile ait yerelleştirilmiş özellikler
        /// </summary>
        public virtual ICollection<LocalizedProperty> LocalizedProperties { get; set; } = new List<LocalizedProperty>();
    }
}
