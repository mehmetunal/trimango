using Trimango.Data.Mssql;

namespace Trimango.Data.Mssql.Entities
{
    /// <summary>
    /// Mesajlaşma entity'si (Müşteri ↔️ Tedarikçi)
    /// </summary>
    public class Message : BaseEntity
    {
        public string Subject { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public bool IsRead { get; set; } = false;
        public DateTime? ReadAt { get; set; }
        public Guid SenderId { get; set; }
        public Guid ReceiverId { get; set; }
        public Guid? PropertyId { get; set; } // Hangi property hakkında
        public Guid? ReservationId { get; set; } // Hangi rezervasyon hakkında
        public Guid? ParentMessageId { get; set; } // Yanıt mesajı ise
        
        // Navigation Properties
        public virtual ApplicationUser Sender { get; set; } = null!;
        public virtual ApplicationUser Receiver { get; set; } = null!;
        public virtual Property? Property { get; set; }
        public virtual Reservation? Reservation { get; set; }
        public virtual Message? ParentMessage { get; set; }
        public virtual ICollection<Message> Replies { get; set; } = new List<Message>();
    }
}
