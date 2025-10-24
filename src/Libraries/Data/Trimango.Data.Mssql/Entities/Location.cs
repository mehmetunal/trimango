using Trimango.Data.Mssql;

namespace Trimango.Data.Mssql.Entities
{
    /// <summary>
    /// Konum entity'si
    /// </summary>
    public class Location : BaseEntity
    {
        public Guid CityId { get; set; }
        public Guid DistrictId { get; set; }
        public string Region { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        
        // Navigation Properties
        public virtual City City { get; set; } = null!;
        public virtual District District { get; set; } = null!;
        public virtual ICollection<Property> Properties { get; set; } = new List<Property>();
    }
}
