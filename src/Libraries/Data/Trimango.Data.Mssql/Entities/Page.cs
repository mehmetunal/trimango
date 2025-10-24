using Trimango.Data.Mssql;

namespace Trimango.Data.Mssql.Entities
{
    /// <summary>
    /// Statik sayfa entity'si (Hakkımızda, SSS, KVKK vb.)
    /// </summary>
    public class Page : BaseEntity
    {
        public string Title { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string MetaTitle { get; set; } = string.Empty;
        public string MetaDescription { get; set; } = string.Empty;
        public string MetaKeywords { get; set; } = string.Empty;
        public string Template { get; set; } = "Default";
        
        // Navigation Properties
        public virtual ICollection<PageComment> Comments { get; set; } = new List<PageComment>();
    }
}
