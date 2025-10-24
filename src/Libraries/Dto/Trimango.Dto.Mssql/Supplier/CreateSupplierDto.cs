using Trimango.Data.Mssql.Entities;

namespace Trimango.Dto.Mssql.Supplier
{
    /// <summary>
    /// Tedarikçi oluşturma DTO'su
    /// </summary>
    public class CreateSupplierDto
    {
        public string Name { get; set; } = string.Empty;
        public string TaxNumber { get; set; } = string.Empty;
        public string IBAN { get; set; } = string.Empty;
        public string ContractStatus { get; set; } = string.Empty;
        public decimal Score { get; set; }
    }
}
