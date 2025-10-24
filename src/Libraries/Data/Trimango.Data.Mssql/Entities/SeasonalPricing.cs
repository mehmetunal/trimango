using Trimango.Data.Mssql;
using Trimango.Data.Mssql.Enums;

namespace Trimango.Data.Mssql.Entities
{
    /// <summary>
    /// Sezonluk fiyatlandırma entity'si
    /// </summary>
    public class SeasonalPricing : BaseEntity
    {
        public ScopeType Scope { get; set; } = ScopeType.Property;
        public Guid ScopeId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal PricePerNight { get; set; }
        public string Currency { get; set; } = "TRY";
        public string Name { get; set; } = string.Empty; // Sezon adı (Yaz, Kış, vb.)
        
        // Navigation Properties
        public virtual Property? Property { get; set; }
        public virtual Unit? Unit { get; set; }
    }
}
