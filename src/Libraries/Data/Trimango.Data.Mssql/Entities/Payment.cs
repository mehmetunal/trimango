using Trimango.Data.Mssql;

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
        public string Method { get; set; } = string.Empty; // CreditCard, BankTransfer, vb.
        public string Status { get; set; } = string.Empty; // Paid, Failed, Pending
        public string TransactionRef { get; set; } = string.Empty;
        public DateTime? PaidAt { get; set; }
        
        // Navigation Properties
        public virtual Reservation Reservation { get; set; } = null!;
    }
}
