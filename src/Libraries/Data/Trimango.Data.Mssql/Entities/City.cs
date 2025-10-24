using Trimango.Data.Mssql;

namespace Trimango.Data.Mssql.Entities
{
    /// <summary>
    /// Şehir bilgilerini temsil eden entity
    /// Property sisteminde şehir bazlı filtreleme için kullanılır
    /// </summary>
    public class City : BaseEntity
    {
        /// <summary>
        /// Şehir adı
        /// </summary>
        public string Name { get; set; } = string.Empty;

        // Navigation Properties
        /// <summary>
        /// Şehre ait ilçeler
        /// </summary>
        public virtual ICollection<District> Districts { get; set; } = new List<District>();

        /// <summary>
        /// Şehirdeki property'ler
        /// </summary>
        public virtual ICollection<Property> Properties { get; set; } = new List<Property>();
        
        /// <summary>
        /// Şehirdeki konumlar
        /// </summary>
        public virtual ICollection<Location> Locations { get; set; } = new List<Location>();
    }
}
