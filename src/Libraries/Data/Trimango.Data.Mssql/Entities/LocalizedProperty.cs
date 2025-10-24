using Trimango.Data.Mssql;
using Trimango.Data.Mssql.Enums;

namespace Trimango.Data.Mssql.Entities
{
    /// <summary>
    /// Entity'lere ait yerelleştirilmiş özellikleri temsil eden entity
    /// Property başlığı, açıklaması gibi çoklu dil desteği gerektiren alanlar için kullanılır
    /// </summary>
    public class LocalizedProperty : BaseEntity
    {
        /// <summary>
        /// Ait olduğu entity'nin ID'si (Property, Category vb.)
        /// </summary>
        public Guid EntityId { get; set; }

        /// <summary>
        /// Ait olduğu dilin ID'si
        /// </summary>
        public Guid LanguageId { get; set; }

        /// <summary>
        /// Yerelleştirme anahtar grubu
        /// </summary>
        public LocaleKeyGroup LocaleKeyGroup { get; set; } = LocaleKeyGroup.Common;

        /// <summary>
        /// Yerelleştirme anahtarı (Title, Description, Name vb.)
        /// </summary>
        public string LocaleKey { get; set; } = string.Empty;

        /// <summary>
        /// Yerelleştirme değeri
        /// </summary>        
        public string LocaleValue { get; set; } = string.Empty;

        // Navigation Properties
        /// <summary>
        /// Ait olduğu dil
        /// </summary>
        public virtual Language Language { get; set; } = null!;
    }
}
