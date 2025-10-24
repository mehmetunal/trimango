using Maggsoft.Core.Base;
using Maggsoft.Core.IoC;
using Maggsoft.Mssql.Repository;
using Trimango.Data.Mssql.Entities;
using Trimango.Mssql.Services.Interfaces;
using Trimango.Dto.Mssql.PropertyType;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace Trimango.Mssql.Services.Services
{
    /// <summary>
    /// Konaklama türü işlemleri için servis implementasyonu
    /// </summary>
    public class PropertyTypeService : CustomBaseService, IService, IPropertyTypeService
    {
        private readonly IMssqlRepository<PropertyType> _propertyTypeRepository;
        private readonly IMapper _mapper;

        public PropertyTypeService(
            IMssqlRepository<PropertyType> propertyTypeRepository,
            IMapper mapper)
        {
            _propertyTypeRepository = propertyTypeRepository;
            _mapper = mapper;
        }

        public async Task<List<PropertyTypeDto>> GetAllPropertyTypesAsync()
        {
            var propertyTypes = await _propertyTypeRepository.Table
                .Where(pt => pt.IsActive && !pt.IsDeleted)
                .OrderBy(pt => pt.Name)
                .ToListAsync();
            return _mapper.Map<List<PropertyTypeDto>>(propertyTypes);
        }

        public async Task<PropertyTypeDto?> GetPropertyTypeByIdAsync(Guid id)
        {
            var propertyType = await _propertyTypeRepository.Table
                .FirstOrDefaultAsync(pt => pt.Id == id && pt.IsActive && !pt.IsDeleted);
            if (propertyType == null) return null;
            
            return _mapper.Map<PropertyTypeDto>(propertyType);
        }

        public async Task<Result<PropertyTypeDto>> CreatePropertyTypeAsync(CreatePropertyTypeDto dto)
        {
            try
            {
                var propertyType = _mapper.Map<PropertyType>(dto);
                propertyType.CreatedDate = DateTime.UtcNow;
                propertyType.CreatorUserId = CurrentUserId ?? Guid.Empty;
                propertyType.IsActive = true;
                propertyType.IsDeleted = false;

                await _propertyTypeRepository.AddAsync(propertyType);
                await _propertyTypeRepository.SaveChangesAsync();

                var result = _mapper.Map<PropertyTypeDto>(propertyType);
                return Success(result);
            }
            catch (Exception ex)
            {
                return OperationFailed<PropertyTypeDto>("Error.PropertyTypeCreationException", new object[] { ex.Message });
            }
        }

        public async Task<Result<PropertyTypeDto>> UpdatePropertyTypeAsync(Guid id, UpdatePropertyTypeDto dto)
        {
            try
            {
                var propertyType = await _propertyTypeRepository.Table
                    .FirstOrDefaultAsync(pt => pt.Id == id && pt.IsActive && !pt.IsDeleted);
                if (propertyType == null)
                    return PropertyTypeNotFound<PropertyTypeDto>();

                _mapper.Map(dto, propertyType);
                propertyType.UpdatedDate = DateTime.UtcNow;
                propertyType.UpdatedByUserId = CurrentUserId;
                propertyType.UpdatedIP = RemoteIp;

                await _propertyTypeRepository.UpdateAsync(propertyType);
                await _propertyTypeRepository.SaveChangesAsync();

                var result = _mapper.Map<PropertyTypeDto>(propertyType);
                return Success(result);
            }
            catch (Exception ex)
            {
                return OperationFailed<PropertyTypeDto>("Error.PropertyTypeUpdateException", new object[] { ex.Message });
            }
        }

        public async Task<Result<bool>> DeletePropertyTypeAsync(Guid id)
        {
            try
            {
                var propertyType = await _propertyTypeRepository.Table
                    .FirstOrDefaultAsync(pt => pt.Id == id && pt.IsActive && !pt.IsDeleted);
                if (propertyType == null)
                    return PropertyTypeNotFound<bool>();

                propertyType.SoftDelete();
                propertyType.UpdatedDate = DateTime.UtcNow;
                propertyType.UpdatedByUserId = CurrentUserId;
                propertyType.UpdatedIP = RemoteIp;

                await _propertyTypeRepository.UpdateAsync(propertyType);
                await _propertyTypeRepository.SaveChangesAsync();

                return Success(true);
            }
            catch (Exception ex)
            {
                return OperationFailed<bool>("Error.PropertyTypeDeleteException", new object[] { ex.Message });
            }
        }
    }
}
