using Trimango.Data.Mssql.Entities;

namespace Trimango.Dto.Mssql.Property
{
    /// <summary>
    /// Konaklama detay DTO'su
    /// </summary>
    public class PropertyDto
    {
        public Guid Id { get; set; }
        public Guid SupplierId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public Guid PropertyTypeId { get; set; }
        public Guid LocationId { get; set; }
        public int Capacity { get; set; }
        public int RoomCount { get; set; }
        public int BathroomCount { get; set; }
        public int SquareMeter { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid? CreatorUserId { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public Guid? UpdatedByUserId { get; set; }
        public string? UpdatedIP { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        
        // Navigation Properties
        public string SupplierName { get; set; } = string.Empty;
        public string PropertyTypeName { get; set; } = string.Empty;
        public string LocationCity { get; set; } = string.Empty;
        public string LocationDistrict { get; set; } = string.Empty;
        public int UnitsCount { get; set; }
        public int ImagesCount { get; set; }
        public int ReservationsCount { get; set; }
    }
}
