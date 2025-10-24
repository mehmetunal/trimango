using Trimango.Data.Mssql;
using Trimango.Data.Mssql.Enums;

namespace Trimango.Data.Mssql.Entities
{
    /// <summary>
    /// Ek ücretler entity'si (Temizlik, Depozito vb.)
    /// </summary>
    public class ExtraFee : BaseEntity
    {
        public ScopeType Scope { get; set; } = ScopeType.Property;
        public Guid ScopeId { get; set; }
        public string Name { get; set; } = string.Empty; // Temizlik Ücreti, Depozito vb.
        public ExtraFeeType Type { get; set; } = ExtraFeeType.Flat;
        public decimal Amount { get; set; }
        public bool Mandatory { get; set; } = false; // Zorunlu mu?
        public string Description { get; set; } = string.Empty;
        
        // Navigation Properties
        public virtual Property? Property { get; set; }
        public virtual Unit? Unit { get; set; }
    }
}
