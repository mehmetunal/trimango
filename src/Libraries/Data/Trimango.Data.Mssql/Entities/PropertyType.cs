using Trimango.Data.Mssql;

namespace Trimango.Data.Mssql.Entities
{
    /// <summary>
    /// Konaklama türü entity'si (Villa, Bungalov, Apart)
    /// </summary>
    public class PropertyType : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string IconUrl { get; set; } = string.Empty;
        public new int DisplayOrder { get; set; }
        
        // Navigation Properties
        public virtual ICollection<Property> Properties { get; set; } = new List<Property>();
    }
}
