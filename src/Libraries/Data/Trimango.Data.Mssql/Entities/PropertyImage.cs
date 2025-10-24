using Trimango.Data.Mssql;

namespace Trimango.Data.Mssql.Entities
{
    /// <summary>
    /// Property görsel entity'si
    /// </summary>
    public class PropertyImage : BaseEntity
    {
        public Guid PropertyId { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public string AltText { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty; // LivingRoom, Kitchen, Exterior, vb.
        public bool IsMain { get; set; }
        public int Order { get; set; }
        
        // Navigation Properties
        public virtual Property Property { get; set; } = null!;
    }
}
