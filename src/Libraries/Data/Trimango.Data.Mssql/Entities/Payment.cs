using Trimango.Data.Mssql;
using Trimango.Data.Mssql.Enums;

namespace Trimango.Data.Mssql.Entities
{
    /// <summary>
    /// Ödeme entity'si
    /// </summary>
    public class Payment : BaseEntity
    {
        public Guid ReservationId { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; } = "TRY";
        public PaymentMethod Method { get; set; } = PaymentMethod.CreditCard;
        public PaymentStatus Status { get; set; } = PaymentStatus.Pending;
        public string TransactionRef { get; set; } = string.Empty;
        public DateTime? PaidAt { get; set; }
        
        // Navigation Properties
        public virtual Reservation Reservation { get; set; } = null!;
    }
}
