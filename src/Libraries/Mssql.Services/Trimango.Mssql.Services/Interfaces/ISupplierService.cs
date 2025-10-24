using Maggsoft.Core.Base;
using Maggsoft.Core.IoC;
using Trimango.Data.Mssql.Entities;
using Trimango.Dto.Mssql.Supplier;

namespace Trimango.Mssql.Services.Interfaces
{
    /// <summary>
    /// Tedarikçi işlemleri için servis interface'i
    /// </summary>
    public interface ISupplierService : IService
    {
        /// <summary>
        /// Tüm tedarikçileri getirir
        /// </summary>
        Task<List<SupplierDto>> GetAllSuppliersAsync();

        /// <summary>
        /// ID'ye göre tedarikçi getirir
        /// </summary>
        Task<SupplierDto?> GetSupplierByIdAsync(Guid id);

        /// <summary>
        /// Yeni tedarikçi oluşturur
        /// </summary>
        Task<Result<SupplierDto>> CreateSupplierAsync(CreateSupplierDto dto);

        /// <summary>
        /// Tedarikçi günceller
        /// </summary>
        Task<Result<SupplierDto>> UpdateSupplierAsync(Guid id, UpdateSupplierDto dto);

        /// <summary>
        /// Tedarikçiyi siler (soft delete)
        /// </summary>
        Task<Result<bool>> DeleteSupplierAsync(Guid id);
    }
}
