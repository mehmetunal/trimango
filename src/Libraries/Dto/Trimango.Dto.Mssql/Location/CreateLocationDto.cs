using Trimango.Data.Mssql.Entities;

namespace Trimango.Dto.Mssql.Location
{
    /// <summary>
    /// Konum oluşturma DTO'su
    /// </summary>
    public class CreateLocationDto
    {
        public string City { get; set; } = string.Empty;
        public string District { get; set; } = string.Empty;
        public string Region { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
    }
}
