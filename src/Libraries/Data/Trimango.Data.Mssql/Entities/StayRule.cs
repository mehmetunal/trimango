using Trimango.Data.Mssql;
using Trimango.Data.Mssql.Enums;

namespace Trimango.Data.Mssql.Entities
{
    /// <summary>
    /// Rezervasyon kuralları entity'si
    /// </summary>
    public class StayRule : BaseEntity
    {
        public ScopeType Scope { get; set; } = ScopeType.Property;
        public Guid ScopeId { get; set; }
        public int MinNights { get; set; }
        public int MaxNights { get; set; }
        public string CheckInDays { get; set; } = string.Empty; // Cumartesi, Pazar vb.
        public int GapNights { get; set; } // Boşluk geceleri
        
        // Navigation Properties
        public virtual Property? Property { get; set; }
        public virtual Unit? Unit { get; set; }
    }
}
