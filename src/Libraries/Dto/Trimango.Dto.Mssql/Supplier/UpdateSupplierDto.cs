using Trimango.Data.Mssql.Entities;

namespace Trimango.Dto.Mssql.Supplier
{
    /// <summary>
    /// Tedarikçi güncelleme DTO'su
    /// </summary>
    public class UpdateSupplierDto
    {
        public string Name { get; set; } = string.Empty;
        public string TaxNumber { get; set; } = string.Empty;
        public string IBAN { get; set; } = string.Empty;
        public string ContractStatus { get; set; } = string.Empty;
        public decimal Score { get; set; }
    }
}
