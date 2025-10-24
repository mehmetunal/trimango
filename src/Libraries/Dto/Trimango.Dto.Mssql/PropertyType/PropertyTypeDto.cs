using Trimango.Data.Mssql.Entities;

namespace Trimango.Dto.Mssql.PropertyType
{
    /// <summary>
    /// Konaklama türü DTO'su
    /// </summary>
    public class PropertyTypeDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string IconUrl { get; set; } = string.Empty;
        public int DisplayOrder { get; set; }
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
