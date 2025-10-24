using Trimango.Data.Mssql;
using Trimango.Data.Mssql.Enums;

namespace Trimango.Data.Mssql.Entities
{
    /// <summary>
    /// Rezervasyon entity'si
    /// </summary>
    public class Reservation : BaseEntity
    {
        public Guid UnitId { get; set; }
        public Guid PropertyId { get; set; }
        public Guid UserId { get; set; } // AspNetUsers ID
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public int GuestCount { get; set; }
        public decimal TotalPrice { get; set; }
        public string Currency { get; set; } = "TRY";
        public ReservationStatus Status { get; set; } = ReservationStatus.Pending;
        public Guid? PolicyId { get; set; }
        public string GuestName { get; set; } = string.Empty;
        public string GuestEmail { get; set; } = string.Empty;
        public string GuestPhone { get; set; } = string.Empty;
        public string SpecialRequests { get; set; } = string.Empty;
        public DateTime? ConfirmedAt { get; set; }
        public DateTime? CancelledAt { get; set; }
        public string CancellationReason { get; set; } = string.Empty;
        
        // Navigation Properties
        public virtual Unit Unit { get; set; } = null!;
        public virtual Property Property { get; set; } = null!;
        public virtual Policy? Policy { get; set; }
        public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
        public virtual ICollection<ReservationPriceBreakdown> PriceBreakdowns { get; set; } = new List<ReservationPriceBreakdown>();
    }
}
