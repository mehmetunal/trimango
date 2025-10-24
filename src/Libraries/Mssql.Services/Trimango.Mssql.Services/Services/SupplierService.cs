using Maggsoft.Core.Base;
using Maggsoft.Core.IoC;
using Maggsoft.Mssql.Repository;
using Trimango.Data.Mssql.Entities;
using Trimango.Mssql.Services.Interfaces;
using Trimango.Dto.Mssql.Supplier;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace Trimango.Mssql.Services.Services
{
    /// <summary>
    /// Tedarikçi işlemleri için servis implementasyonu
    /// </summary>
    public class SupplierService : CustomBaseService, IService, ISupplierService
    {
        private readonly IMssqlRepository<Supplier> _supplierRepository;
        private readonly IMapper _mapper;

        public SupplierService(
            IMssqlRepository<Supplier> supplierRepository,
            IMapper mapper)
        {
            _supplierRepository = supplierRepository;
            _mapper = mapper;
        }

        public async Task<List<SupplierDto>> GetAllSuppliersAsync()
        {
            var suppliers = await _supplierRepository.Table
                .Where(s => s.IsActive && !s.IsDeleted)
                .OrderBy(s => s.Name)
                .ToListAsync();
            return _mapper.Map<List<SupplierDto>>(suppliers);
        }

        public async Task<SupplierDto?> GetSupplierByIdAsync(Guid id)
        {
            var supplier = await _supplierRepository.Table
                .FirstOrDefaultAsync(s => s.Id == id && s.IsActive && !s.IsDeleted);
            if (supplier == null) return null;
            
            return _mapper.Map<SupplierDto>(supplier);
        }

        public async Task<Result<SupplierDto>> CreateSupplierAsync(CreateSupplierDto dto)
        {
            try
            {
                var supplier = _mapper.Map<Supplier>(dto);
                supplier.CreatedDate = DateTime.UtcNow;
                supplier.CreatorUserId = CurrentUserId ?? Guid.Empty;
                supplier.IsActive = true;
                supplier.IsDeleted = false;

                await _supplierRepository.AddAsync(supplier);
                await _supplierRepository.SaveChangesAsync();

                var result = _mapper.Map<SupplierDto>(supplier);
                return Success(result);
            }
            catch (Exception ex)
            {
                return OperationFailed<SupplierDto>("Error.SupplierCreationException", new object[] { ex.Message });
            }
        }

        public async Task<Result<SupplierDto>> UpdateSupplierAsync(Guid id, UpdateSupplierDto dto)
        {
            try
            {
                var supplier = await _supplierRepository.Table
                    .FirstOrDefaultAsync(s => s.Id == id && s.IsActive && !s.IsDeleted);
                if (supplier == null)
                    return SupplierNotFound<SupplierDto>();

                _mapper.Map(dto, supplier);
                supplier.UpdatedDate = DateTime.UtcNow;
                supplier.UpdatedByUserId = CurrentUserId;
                supplier.UpdatedIP = RemoteIp;

                await _supplierRepository.UpdateAsync(supplier);
                await _supplierRepository.SaveChangesAsync();

                var result = _mapper.Map<SupplierDto>(supplier);
                return Success(result);
            }
            catch (Exception ex)
            {
                return OperationFailed<SupplierDto>("Error.SupplierUpdateException", new object[] { ex.Message });
            }
        }

        public async Task<Result<bool>> DeleteSupplierAsync(Guid id)
        {
            try
            {
                var supplier = await _supplierRepository.Table
                    .FirstOrDefaultAsync(s => s.Id == id && s.IsActive && !s.IsDeleted);
                if (supplier == null)
                    return SupplierNotFound<bool>();

                supplier.SoftDelete();
                supplier.UpdatedDate = DateTime.UtcNow;
                supplier.UpdatedByUserId = CurrentUserId;
                supplier.UpdatedIP = RemoteIp;

                await _supplierRepository.UpdateAsync(supplier);
                await _supplierRepository.SaveChangesAsync();

                return Success(true);
            }
            catch (Exception ex)
            {
                return OperationFailed<bool>("Error.SupplierDeleteException", new object[] { ex.Message });
            }
        }
    }
}
