using Trimango.Data.Mssql.Entities;

namespace Trimango.Dto.Mssql.Supplier
{
    /// <summary>
    /// Tedarikçi liste DTO'su
    /// </summary>
    public class SupplierListDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string ContractStatus { get; set; } = string.Empty;
        public decimal Score { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsActive { get; set; }
        
        // Navigation Properties
        public int PropertiesCount { get; set; }
    }
}
