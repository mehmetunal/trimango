using Trimango.Data.Mssql;

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
        public string Status { get; set; } = string.Empty; // Pending, Confirmed, Cancelled, vb.
        public Guid? PolicyId { get; set; }
        
        // Navigation Properties
        public virtual Unit Unit { get; set; } = null!;
        public virtual Property Property { get; set; } = null!;
        public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
    }
}
