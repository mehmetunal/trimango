using Trimango.Data.Mssql;

namespace Trimango.Data.Mssql.Entities
{
    /// <summary>
    /// Kullanıcı yorumu entity'si
    /// </summary>
    public class Review : BaseEntity
    {
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public int Rating { get; set; } // 1-5 arası
        public bool IsApproved { get; set; } = false;
        public bool IsVerified { get; set; } = false; // Doğrulanmış misafir
        public Guid PropertyId { get; set; }
        public Guid UserId { get; set; }
        public Guid? ReservationId { get; set; } // Hangi rezervasyon için yorum
        
        // Navigation Properties
        public virtual Property Property { get; set; } = null!;
        public virtual ApplicationUser User { get; set; } = null!;
        public virtual Reservation? Reservation { get; set; }
    }
}
