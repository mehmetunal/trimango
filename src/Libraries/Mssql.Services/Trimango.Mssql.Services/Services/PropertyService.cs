using Maggsoft.Core.Base;
using Maggsoft.Core.IoC;
using Maggsoft.Mssql.Repository;
using Trimango.Data.Mssql.Entities;
using Trimango.Mssql.Services.Interfaces;
using Trimango.Dto.Mssql.Property;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace Trimango.Mssql.Services.Services
{
    /// <summary>
    /// Konaklama işlemleri için servis implementasyonu
    /// </summary>
    public class PropertyService : CustomBaseService, IService, IPropertyService
    {
        private readonly IMssqlRepository<Property> _propertyRepository;
        private readonly IMapper _mapper;

        public PropertyService(
            IMssqlRepository<Property> propertyRepository,
            IMapper mapper)
        {
            _propertyRepository = propertyRepository;
            _mapper = mapper;
        }

        public async Task<List<PropertyDto>> GetAllPropertiesAsync()
        {
            var properties = await _propertyRepository.Table
                .Where(p => p.IsActive && !p.IsDeleted)
                .Include(p => p.PropertyType)
                .Include(p => p.Location)
                .Include(p => p.Supplier)
                .OrderBy(p => p.Title)
                .ToListAsync();
            return _mapper.Map<List<PropertyDto>>(properties);
        }

        public async Task<PropertyDto?> GetPropertyByIdAsync(Guid id)
        {
            var property = await _propertyRepository.Table
                .Where(p => p.Id == id && p.IsActive && !p.IsDeleted)
                .Include(p => p.PropertyType)
                .Include(p => p.Location)
                .Include(p => p.Supplier)
                .FirstOrDefaultAsync();
            if (property == null) return null;
            
            return _mapper.Map<PropertyDto>(property);
        }

        public async Task<List<PropertyDto>> GetPropertiesBySupplierIdAsync(Guid supplierId)
        {
            var properties = await _propertyRepository.Table
                .Where(p => p.SupplierId == supplierId && p.IsActive && !p.IsDeleted)
                .Include(p => p.PropertyType)
                .Include(p => p.Location)
                .Include(p => p.Supplier)
                .OrderBy(p => p.Title)
                .ToListAsync();
            return _mapper.Map<List<PropertyDto>>(properties);
        }

        public async Task<Result<PropertyDto>> CreatePropertyAsync(CreatePropertyDto dto)
        {
            try
            {
                var property = _mapper.Map<Property>(dto);
                property.CreatedDate = DateTime.UtcNow;
                property.CreatorUserId = CurrentUserId ?? Guid.Empty;
                property.IsActive = true;
                property.IsDeleted = false;

                await _propertyRepository.AddAsync(property);
                await _propertyRepository.SaveChangesAsync();

                var result = _mapper.Map<PropertyDto>(property);
                return Success(result);
            }
            catch (Exception ex)
            {
                return OperationFailed<PropertyDto>("Error.PropertyCreationException", new object[] { ex.Message });
            }
        }

        public async Task<Result<PropertyDto>> UpdatePropertyAsync(Guid id, UpdatePropertyDto dto)
        {
            try
            {
                var property = await _propertyRepository.Table
                    .FirstOrDefaultAsync(p => p.Id == id && p.IsActive && !p.IsDeleted);
                if (property == null)
                    return PropertyNotFound<PropertyDto>();

                _mapper.Map(dto, property);
                property.UpdatedDate = DateTime.UtcNow;
                property.UpdatedByUserId = CurrentUserId;
                property.UpdatedIP = RemoteIp;

                await _propertyRepository.UpdateAsync(property);
                await _propertyRepository.SaveChangesAsync();

                var result = _mapper.Map<PropertyDto>(property);
                return Success(result);
            }
            catch (Exception ex)
            {
                return OperationFailed<PropertyDto>("Error.PropertyUpdateException", new object[] { ex.Message });
            }
        }

        public async Task<Result<bool>> DeletePropertyAsync(Guid id)
        {
            try
            {
                var property = await _propertyRepository.Table
                    .FirstOrDefaultAsync(p => p.Id == id && p.IsActive && !p.IsDeleted);
                if (property == null)
                    return PropertyNotFound<bool>();

                property.SoftDelete();
                property.UpdatedDate = DateTime.UtcNow;
                property.UpdatedByUserId = CurrentUserId;
                property.UpdatedIP = RemoteIp;

                await _propertyRepository.UpdateAsync(property);
                await _propertyRepository.SaveChangesAsync();

                return Success(true);
            }
            catch (Exception ex)
            {
                return OperationFailed<bool>("Error.PropertyDeleteException", new object[] { ex.Message });
            }
        }
    }
}
