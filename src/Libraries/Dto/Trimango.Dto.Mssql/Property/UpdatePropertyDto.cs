using Trimango.Data.Mssql.Entities;

namespace Trimango.Dto.Mssql.Property
{
    /// <summary>
    /// Konaklama güncelleme DTO'su
    /// </summary>
    public class UpdatePropertyDto
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public Guid PropertyTypeId { get; set; }
        public Guid LocationId { get; set; }
        public int Capacity { get; set; }
        public int RoomCount { get; set; }
        public int BathroomCount { get; set; }
        public int SquareMeter { get; set; }
    }
}
