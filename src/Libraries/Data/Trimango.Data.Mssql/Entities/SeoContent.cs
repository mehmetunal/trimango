using Trimango.Data.Mssql;
using Trimango.Data.Mssql.Enums;

namespace Trimango.Data.Mssql.Entities
{
    /// <summary>
    /// SEO içerik entity'si (Anasayfa blokları, footer yazıları vb.)
    /// </summary>
    public class SeoContent : BaseEntity
    {
        public string Key { get; set; } = string.Empty; // Unique key (homepage-hero, footer-about, vb.)
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public Guid LanguageId { get; set; } // Language ile ilişki (Foreign Key yok)
        public SeoSection Section { get; set; } = SeoSection.Homepage; // homepage, footer, header, vb.
    }
}
