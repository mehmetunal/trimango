using Trimango.Data.Mssql;
using Trimango.Data.Mssql.Enums;

namespace Trimango.Data.Mssql.Entities
{
    /// <summary>
    /// Rezervasyon anlaşmazlığı entity'si
    /// </summary>
    public class Dispute : BaseEntity
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DisputeStatus Status { get; set; } = DisputeStatus.Open; // Open, InProgress, Resolved, Closed
        public DisputePriority Priority { get; set; } = DisputePriority.Medium; // Low, Medium, High, Critical
        public string Resolution { get; set; } = string.Empty;
        public DateTime? ResolvedAt { get; set; }
        public Guid ReservationId { get; set; }
        public Guid ComplainantId { get; set; } // Şikayet eden
        public Guid? AssignedToUserId { get; set; } // Kim atandı (admin)
        public Guid? ResolvedByUserId { get; set; } // Kim çözdü
        
        // Navigation Properties
        public virtual Reservation Reservation { get; set; } = null!;
        public virtual ApplicationUser Complainant { get; set; } = null!;
        public virtual ApplicationUser? AssignedToUser { get; set; }
        public virtual ApplicationUser? ResolvedByUser { get; set; }
    }
}
