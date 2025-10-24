using Maggsoft.Core.Base;
using Maggsoft.Core.IoC;
using Trimango.Dto.Mssql.PropertyType;

namespace Trimango.Mssql.Services.Interfaces
{
    /// <summary>
    /// Konaklama türü işlemleri için servis arayüzü
    /// </summary>
    public interface IPropertyTypeService : IService
    {
        Task<List<PropertyTypeDto>> GetAllPropertyTypesAsync();
        Task<PropertyTypeDto?> GetPropertyTypeByIdAsync(Guid id);
        Task<Result<PropertyTypeDto>> CreatePropertyTypeAsync(CreatePropertyTypeDto dto);
        Task<Result<PropertyTypeDto>> UpdatePropertyTypeAsync(Guid id, UpdatePropertyTypeDto dto);
        Task<Result<bool>> DeletePropertyTypeAsync(Guid id);
    }
}
