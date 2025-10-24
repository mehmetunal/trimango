using Trimango.Data.Mssql;
using Trimango.Data.Mssql.Enums;

namespace Trimango.Data.Mssql.Entities
{
    /// <summary>
    /// Politika entity'si (İptal, Ev kuralları vb.)
    /// </summary>
    public class Policy : BaseEntity
    {
        public PolicyType PolicyType { get; set; } = PolicyType.Cancel;
        public string Name { get; set; } = string.Empty; // Politika adı
        public string Terms { get; set; } = string.Empty; // Şartlar
        public bool IsVisible { get; set; } = true; // Kullanıcıya gösterilsin mi?
        
        // Navigation Properties
        public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
    }
}
