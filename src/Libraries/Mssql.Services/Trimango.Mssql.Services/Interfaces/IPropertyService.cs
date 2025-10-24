using Maggsoft.Core.Base;
using Maggsoft.Core.IoC;
using Trimango.Dto.Mssql.Property;

namespace Trimango.Mssql.Services.Interfaces
{
    /// <summary>
    /// Konaklama işlemleri için servis interface'i
    /// </summary>
    public interface IPropertyService : IService
    {
        /// <summary>
        /// Tüm konaklamaları getirir
        /// </summary>
        Task<List<PropertyDto>> GetAllPropertiesAsync();

        /// <summary>
        /// ID'ye göre konaklama getirir
        /// </summary>
        Task<PropertyDto?> GetPropertyByIdAsync(Guid id);

        /// <summary>
        /// Tedarikçi ID'sine göre konaklamaları getirir
        /// </summary>
        Task<List<PropertyDto>> GetPropertiesBySupplierIdAsync(Guid supplierId);

        /// <summary>
        /// Yeni konaklama oluşturur
        /// </summary>
        Task<Result<PropertyDto>> CreatePropertyAsync(CreatePropertyDto dto);

        /// <summary>
        /// Konaklama günceller
        /// </summary>
        Task<Result<PropertyDto>> UpdatePropertyAsync(Guid id, UpdatePropertyDto dto);

        /// <summary>
        /// Konaklamayı siler (soft delete)
        /// </summary>
        Task<Result<bool>> DeletePropertyAsync(Guid id);
    }
}
