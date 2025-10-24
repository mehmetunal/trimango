using Trimango.Data.Mssql;
using Trimango.Data.Mssql.Enums;

namespace Trimango.Data.Mssql.Entities
{
    /// <summary>
    /// Uzaklık bilgileri entity'si (Plaj, Havaalanı vb.)
    /// </summary>
    public class DistanceInfo : BaseEntity
    {
        public Guid PropertyId { get; set; }
        public string PlaceName { get; set; } = string.Empty;
        public decimal DistanceKm { get; set; }
        public PlaceType PlaceType { get; set; } = PlaceType.Other;
        
        // Navigation Properties
        public virtual Property Property { get; set; } = null!;
    }
}
