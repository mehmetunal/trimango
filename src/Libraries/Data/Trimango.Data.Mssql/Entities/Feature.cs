using Trimango.Data.Mssql;
using Trimango.Data.Mssql.Enums;

namespace Trimango.Data.Mssql.Entities
{
    /// <summary>
    /// Özellik entity'si (Havuz, Jakuzi, Sauna vb.)
    /// </summary>
    public class Feature : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public FeatureCategory Category { get; set; } = FeatureCategory.General;
        public string IconUrl { get; set; } = string.Empty;
        
        // Navigation Properties
        public virtual ICollection<PropertyFeatureMapping> PropertyFeatureMappings { get; set; } = new List<PropertyFeatureMapping>();
    }
}
