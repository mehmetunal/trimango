using Trimango.Data.Mssql;

namespace Trimango.Data.Mssql.Entities
{
    /// <summary>
    /// İlçe bilgilerini temsil eden entity
    /// Property sisteminde ilçe bazlı filtreleme için kullanılır
    /// </summary>
    public class District : BaseEntity
    {
        /// <summary>
        /// İlçe adı
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// İlçenin ait olduğu şehrin ID'si
        /// </summary>
        public Guid CityId { get; set; }

        // Navigation Properties
        /// <summary>
        /// İlçenin ait olduğu şehir
        /// </summary>
        public virtual City City { get; set; } = null!;

        /// <summary>
        /// İlçedeki property'ler
        /// </summary>
        public virtual ICollection<Property> Properties { get; set; } = new List<Property>();
        
        /// <summary>
        /// İlçedeki konumlar
        /// </summary>
        public virtual ICollection<Location> Locations { get; set; } = new List<Location>();
    }
}
