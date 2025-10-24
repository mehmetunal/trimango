using Trimango.Data.Mssql;

namespace Trimango.Data.Mssql.Entities
{
    /// <summary>
    /// Property-Feature N-N ilişki tablosu
    /// </summary>
    public class PropertyFeatureMapping : BaseEntity
    {
        public Guid PropertyId { get; set; }
        public Guid FeatureId { get; set; }
        
        // Navigation Properties
        public virtual Property Property { get; set; } = null!;
        public virtual Feature Feature { get; set; } = null!;
    }
}
