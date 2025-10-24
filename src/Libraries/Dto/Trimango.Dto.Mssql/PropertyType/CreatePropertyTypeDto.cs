using Trimango.Data.Mssql.Entities;

namespace Trimango.Dto.Mssql.PropertyType
{
    /// <summary>
    /// Konaklama türü oluşturma DTO'su
    /// </summary>
    public class CreatePropertyTypeDto
    {
        public string Name { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string IconUrl { get; set; } = string.Empty;
        public int DisplayOrder { get; set; }
    }
}
