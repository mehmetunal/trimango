using Trimango.Data.Mssql.Entities;

namespace Trimango.Dto.Mssql.Location
{
    /// <summary>
    /// Konum DTO'su
    /// </summary>
    public class LocationDto
    {
        public Guid Id { get; set; }
        public string City { get; set; } = string.Empty;
        public string District { get; set; } = string.Empty;
        public string Region { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid? CreatorUserId { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public Guid? UpdatedByUserId { get; set; }
        public string? UpdatedIP { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        
        // Navigation Properties
        public int PropertiesCount { get; set; }
    }
}
