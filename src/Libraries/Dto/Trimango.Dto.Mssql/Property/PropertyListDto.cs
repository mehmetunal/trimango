using Trimango.Data.Mssql.Entities;

namespace Trimango.Dto.Mssql.Property
{
    /// <summary>
    /// Konaklama liste DTO'su
    /// </summary>
    public class PropertyListDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public Guid PropertyTypeId { get; set; }
        public Guid LocationId { get; set; }
        public int Capacity { get; set; }
        public int RoomCount { get; set; }
        public int BathroomCount { get; set; }
        public int SquareMeter { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsActive { get; set; }
        
        // Navigation Properties
        public string SupplierName { get; set; } = string.Empty;
        public string PropertyTypeName { get; set; } = string.Empty;
        public string LocationCity { get; set; } = string.Empty;
        public string LocationDistrict { get; set; } = string.Empty;
        public int UnitsCount { get; set; }
        public int ImagesCount { get; set; }
    }
}
