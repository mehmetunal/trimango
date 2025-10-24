using Trimango.Data.Mssql;

namespace Trimango.Data.Mssql.Entities
{
    /// <summary>
    /// Favori kayıtları entity'si
    /// </summary>
    public class Favorite : BaseEntity
    {
        public Guid PropertyId { get; set; }
        public Guid UserId { get; set; }
        public string Notes { get; set; } = string.Empty; // Kullanıcı notları
        
        // Navigation Properties
        public virtual Property Property { get; set; } = null!;
        public virtual ApplicationUser User { get; set; } = null!;
    }
}
