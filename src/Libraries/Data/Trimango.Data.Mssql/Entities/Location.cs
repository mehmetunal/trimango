using Trimango.Data.Mssql;

namespace Trimango.Data.Mssql.Entities
{
    /// <summary>
    /// Konum entity'si
    /// </summary>
    public class Location : BaseEntity
    {
        public string City { get; set; } = string.Empty;
        public string District { get; set; } = string.Empty;
        public string Region { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        
        // Navigation Properties
        public virtual ICollection<Property> Properties { get; set; } = new List<Property>();
    }
}
