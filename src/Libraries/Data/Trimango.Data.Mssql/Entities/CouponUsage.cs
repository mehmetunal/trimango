using Trimango.Data.Mssql;

namespace Trimango.Data.Mssql.Entities
{
    /// <summary>
    /// Kupon kullanımı entity'si
    /// </summary>
    public class CouponUsage : BaseEntity
    {
        public Guid CouponId { get; set; }
        public Guid UserId { get; set; }
        public Guid ReservationId { get; set; }
        public decimal DiscountAmount { get; set; }
        public DateTime UsedAt { get; set; } = DateTime.UtcNow;
        
        // Navigation Properties
        public virtual Coupon Coupon { get; set; } = null!;
        public virtual ApplicationUser User { get; set; } = null!;
        public virtual Reservation Reservation { get; set; } = null!;
    }
}
