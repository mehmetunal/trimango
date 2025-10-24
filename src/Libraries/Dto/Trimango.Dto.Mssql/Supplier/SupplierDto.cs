using Trimango.Data.Mssql.Entities;

namespace Trimango.Dto.Mssql.Supplier
{
    /// <summary>
    /// Tedarikçi detay DTO'su
    /// </summary>
    public class SupplierDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string TaxNumber { get; set; } = string.Empty;
        public string IBAN { get; set; } = string.Empty;
        public string ContractStatus { get; set; } = string.Empty;
        public decimal Score { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid? CreatorUserId { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public Guid? UpdatedByUserId { get; set; }
        public string? UpdatedIP { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        
        // Navigation Properties
        public int PropertiesCount { get; set; }
        public int UsersCount { get; set; }
    }
}
