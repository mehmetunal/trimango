using Trimango.Data.Mssql;

namespace Trimango.Data.Mssql.Entities
{
    /// <summary>
    /// Tedarikçi entity'si
    /// </summary>
    public class Supplier : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string TaxNumber { get; set; } = string.Empty;
        public string IBAN { get; set; } = string.Empty;
        public string ContractStatus { get; set; } = string.Empty;
        public decimal Score { get; set; }
        
        // Navigation Properties
        public virtual ICollection<Property> Properties { get; set; } = new List<Property>();
        public virtual ICollection<ApplicationUser> Users { get; set; } = new List<ApplicationUser>();
    }
}
