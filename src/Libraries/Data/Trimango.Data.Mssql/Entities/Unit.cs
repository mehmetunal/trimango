using Trimango.Data.Mssql;

namespace Trimango.Data.Mssql.Entities
{
    /// <summary>
    /// Ünite entity'si (Property içindeki bireysel konaklama birimleri)
    /// </summary>
    public class Unit : BaseEntity
    {
        public Guid PropertyId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Capacity { get; set; }
        public string BedConfig { get; set; } = string.Empty;
        public bool PrivatePool { get; set; }
        public decimal Price { get; set; }
        public string Currency { get; set; } = "TRY";
        
        // Navigation Properties
        public virtual Property Property { get; set; } = null!;
        public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
    }
}
