using Trimango.Data.Mssql;

namespace Trimango.Data.Mssql.Entities
{
    /// <summary>
    /// Dil çeviri kaynaklarını temsil eden entity
    /// Sistem genelinde kullanılan metinlerin çevirilerini içerir
    /// </summary>
    public class LocaleStringResource : BaseEntity
    {
        /// <summary>
        /// Ait olduğu dilin ID'si
        /// </summary>
        public Guid LanguageId { get; set; }

        /// <summary>
        /// Çeviri anahtarı (örn: "Common.Save", "Property.Title" vb.)
        /// </summary>
        public string ResourceName { get; set; } = string.Empty;

        /// <summary>
        /// Çeviri değeri
        /// </summary>        
        public string ResourceValue { get; set; } = string.Empty;

        // Navigation Properties
        /// <summary>
        /// Ait olduğu dil
        /// </summary>
        public virtual Language Language { get; set; } = null!;
    }
}
