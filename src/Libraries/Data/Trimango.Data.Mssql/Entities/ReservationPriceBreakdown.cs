using Trimango.Data.Mssql;
using Trimango.Data.Mssql.Enums;

namespace Trimango.Data.Mssql.Entities
{
    /// <summary>
    /// Rezervasyon fiyat kırılımı entity'si
    /// </summary>
    public class ReservationPriceBreakdown : BaseEntity
    {
        public Guid ReservationId { get; set; }
        public PriceBreakdownType LineType { get; set; } = PriceBreakdownType.Nightly;
        public string Name { get; set; } = string.Empty; // Açıklama
        public int Qty { get; set; } // Adet
        public decimal UnitPrice { get; set; } // Birim fiyat
        public decimal Total { get; set; } // Toplam
        
        // Navigation Properties
        public virtual Reservation Reservation { get; set; } = null!;
    }
}
