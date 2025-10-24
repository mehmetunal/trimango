using Trimango.Data.Mssql;
using Trimango.Data.Mssql.Enums;

namespace Trimango.Data.Mssql.Entities
{
    /// <summary>
    /// Kupon entity'si
    /// </summary>
    public class Coupon : BaseEntity
    {
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public CouponType Type { get; set; } = CouponType.Percentage; // Percentage, FixedAmount
        public decimal Value { get; set; } // Yüzde veya sabit tutar
        public decimal? MinOrderAmount { get; set; } // Minimum sipariş tutarı
        public decimal? MaxDiscountAmount { get; set; } // Maksimum indirim tutarı
        public int UsageLimit { get; set; } = 0; // 0 = sınırsız
        public int UsedCount { get; set; } = 0;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string ApplicableProperties { get; set; } = string.Empty; // JSON array of property IDs
        public string ApplicableUsers { get; set; } = string.Empty; // JSON array of user IDs (özel kuponlar)
        
        // Navigation Properties
        public virtual ICollection<CouponUsage> Usages { get; set; } = new List<CouponUsage>();
    }
}
