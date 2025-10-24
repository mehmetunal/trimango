using Trimango.Data.Mssql;

namespace Trimango.Data.Mssql.Entities
{
    /// <summary>
    /// Konaklama tesis entity'si
    /// </summary>
    public class Property : BaseEntity
    {
        public Guid SupplierId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public Guid PropertyTypeId { get; set; }
        public Guid LocationId { get; set; }
        public int Capacity { get; set; }
        public int RoomCount { get; set; }
        public int BathroomCount { get; set; }
        public int SquareMeter { get; set; }
        
        // Navigation Properties
        public virtual Supplier Supplier { get; set; } = null!;
        public virtual PropertyType PropertyType { get; set; } = null!;
        public virtual Location Location { get; set; } = null!;
        public virtual ICollection<Unit> Units { get; set; } = new List<Unit>();
        public virtual ICollection<PropertyImage> PropertyImages { get; set; } = new List<PropertyImage>();
        public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
    }
}
