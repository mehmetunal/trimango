using Trimango.Data.Mssql;
using Trimango.Data.Mssql.Enums;

namespace Trimango.Data.Mssql.Entities
{
    /// <summary>
    /// Bildirim entity'si
    /// </summary>
    public class Notification : BaseEntity
    {
        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public NotificationType Type { get; set; } = NotificationType.Info; // Info, Success, Warning, Error
        public bool IsRead { get; set; } = false;
        public DateTime? ReadAt { get; set; }
        public string ActionUrl { get; set; } = string.Empty; // Tıklanınca gidilecek URL
        public string IconUrl { get; set; } = string.Empty;
        public Guid UserId { get; set; }
        public Guid? RelatedEntityId { get; set; } // İlgili entity ID'si
        public string RelatedEntityType { get; set; } = string.Empty; // Property, Reservation, Payment vb.
        
        // Navigation Properties
        public virtual ApplicationUser User { get; set; } = null!;
    }
}
